using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Color = UnityEngine.Color;

// 다른클래스에서 비교할 땐 IComparer<T>
// 해당 클래스에서 비교할 땐 IComparable<T>
public class HeuristicComparer : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        if (x == null || y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if (x.weight < y.weight) return -1;
        if (x.weight > y.weight) return 1;
        return 0;
        // return x.Heuristic.CompareTo(y.heurustic)과 같음 
    }
}

public class AStar : MonoBehaviour
{
    [Header("===Object===")]
    [SerializeField] private GameObject _testObject;
    [SerializeField] private GameObject _testUnit;
    
    [Header("===Var===")]
    private bool[,] _mapGrid;
    private int _mapSize;
    private int _chunk = 10;

    [Header("===Container===")]
    private bool[,] visit;
    private int[,] heuristic;
    private Pos[,] pathFinder;
    private List<Pos> path;

    [Header("===Direct===")]
    private int[] dx = new int[4] { 0,1,0,-1 };
    private int[] dy = new int[4] { 1,0,-1,0 };

    // 우선순위큐 대신에 사용할 List
    [Header("===Priority List===")]
    List<Node> _nodes;
    //PriorityQueue<Node, int> _priority = new PriorityQueue<Node, int>();
    //  좌표 , 현재까지 거리 g + 휴리스틱 h (정렬 기준은 g+h )

    private void Start()
    {
        // 얕은복사
        _mapGrid    = MapManager.instance.mapGrid;

        _mapSize    = MapManager.instance.mapSize;

        visit       = new bool[_chunk * 2, _chunk * 2];
        heuristic   = new int[_chunk * 2, _chunk * 2];

        _nodes      = new List<Node>();

        pathFinder = new Pos[_chunk * 2, _chunk * 2];
        path = new List<Pos>();

        Debug.Log("테스트 유닛 위치 " +_testUnit.transform.position.x + " / " + _testUnit.transform.position.z);

        //대충mapgrid 출력
        /*
        for (int y = 0; y < 140; y++) 
        {
            for (int x = 0; x < 140; x++) 
            {
                // 방문했으면 false
                if (_mapGrid[y,x] == false)
                    continue;

                GameObject _te = Instantiate(_testObject , new Vector3(x,0,y) , Quaternion.identity);
            }
        }
        */

        // ##TODO
        // 임시 unit으로 해보기 
        F_SearchPath(_testUnit.transform.position, new Vector3(2, 1, 3));

    }

    private void Update()
    {
        Debug.DrawRay(new Vector3(20, 1, 45), (new Vector3(2, 1, 3) - new Vector3(20, 1, 45)).normalized * 100f , Color.blue);
    }

    public void F_SearchPath(Vector3 _start, Vector3 _goal) 
    {

        // 청크(20x20)만큼 배열들 초기화
        F_InitContainer( (int)_start.x, (int)_start.z);

        // 청크 내 도착지 정하기 
        Vector3 desti = F_FindChunckDestination( _start , _goal );

        Debug.Log("도착지 : " + desti.x +  " , " + desti.z);

        // A*로 길찾기 
        F_FindAStarPath( (int)_start.x , (int)_start.z , (int)desti.x , (int)desti.z);

        // 이동경로 정하기 
        F_CheckPath((int)_start.x, (int)_start.z, (int)desti.x, (int)desti.z);
    }

    private void F_FindAStarPath( int startX, int startY, int goalX, int goalY) 
    {
        // node리스트에 추가 
        Node startNode = new Node( startX, startY, 0, F_Heuristic(startX, startY, goalX, goalY));
        _nodes.Add(startNode);

        // 방문처리 
        visit[0, 0] = true;

        // 휴리스틱 설정 
        heuristic[0, 0] = startNode.weight;

        // 현재 좌표에 대한 max
        int MinX = Math.Max(startX - _chunk, 0);
        int MaxX = Math.Min(startX + _chunk, _mapSize);
        int MinY = Math.Max(startY - _chunk, 0);
        int MaxY = Math.Min(startY + _chunk, _mapSize);


        //Debug.Log(MinX + " , " + MinY + " / " + MaxX + " , " + MaxY);

        // 방문처리 잘되있는지 출력 (인덱스 참고!@! )
        /*
        for (int y = 0; y < 20; y++) 
        {
            for (int x = 0; x < 20; x++) 
            {
                int nowX = startX - _chunk + x;
                int nowY = startY - _chunk + y; 

                if(nowX < 0 || nowY < 0)
                    continue;

                // 방문을 안 했으면 
                if (_mapGrid[nowY ,nowX] == true)
                {
                    GameObject temp = Instantiate(_testObject, new Vector3(startX - _chunk + x, 2 , startY - _chunk + y), Quaternion.identity);
                    temp.gameObject.name = y.ToString() + " / " + x.ToString();
                }
            }
        }
        */

        while (true) 
        {
            if (_nodes.Count == 0)
                break;

            // 현재 노드 
            _nodes.Sort(new HeuristicComparer());  // 내가 정한 기준으로 sort 됨 
            Node _curr = _nodes[0];
            _nodes.RemoveAt(0);

            int _currX = _curr.x; 
            int _currY = _curr.y;
            int _dis = _curr.distance; 
            int _heuri = _curr.weight;
             
            //도착하면?
            if (_currX == goalX && _currY == goalY)
                break;

            // 8방향으로
            for (int i = 0; i < 4; i++) 
            {
                int nx = _currX + dx[i];
                int ny = _currY + dy[i];

                // 올바른 좌표에 있는지 
                if (nx < MinX || ny < MinY || nx >= MaxX || ny >= MaxY)
                {
                    //Debug.Log("1에서 continew");
                    continue;
                }

                // 방문했는지 (방문했으면 false)
                int norX = nx - (startX - _chunk);
                int norY = ny - (startY - _chunk);
                if (visit[norY , norX] == false)
                {
                    //Debug.Log("2에서 continue");
                    continue;
                }

                //  g + h가 저장된 휴리스틱보다 크면?
                int nowDis = _dis + 1;
                int nowHeuri = F_Heuristic( nx , ny , goalX , goalY);

                //Debug.Log( "(" + norY + " , " + norX + ") "+ "/ 현재 : " + (nowDis + nowHeuri).ToString() + " / 저장된 휴리스틱 : " + heuristic[norY, norX]);
                if (nowDis + nowHeuri > heuristic[norY, norX])
                {
                    //Debug.Log("3에서 continue"); 
                    continue;
                }

                // 조건충족하면
                // List에 넣기 
                Node _nextNode = new Node(nx, ny, nowDis , nowHeuri);
                _nodes.Add(_nextNode);

                // 방문처리 
                visit[norY , norX ] = false;

                // 가중치(d+h) 넣기 
                heuristic[norY , norX] = nowDis + nowHeuri;

                // 경로잡기
                pathFinder[ norY , norX ] 
                    = new Pos(_currY - (startY - _chunk) , _currX - (startX - _chunk));

            }

        }
    }

    private void F_CheckPath(int startX, int startY ,int goalX, int goalY) 
    {
        int curX = goalX;
        int curY = goalY;

        int cx = curX - (startX - _chunk);
        int cy = curY - (startY - _chunk);

        Debug.Log( "도착지 보정 : "+ cx + " , " + cy);

        if (pathFinder[cx , cy] == null)
            Debug.LogError("tlqkf");

       


        /*
        while (true)
        {
            Debug.Log("현재 ( " + curX + " , " + curY + ")");

            int cx = curX - (startX - _chunk);
            int cy = curY - (startY - _chunk);

            if ( curX == startX && curY == startY)
                break;

            path.Add(new Pos(curY , curX));

            curX = temp.x;
            curY = temp.y;

        }
        */

        // 맨 마치막 : 시작점 리스트에 담기
        path.Add(new Pos(curY , curX));
        // 현재 끝->시작으로 담았으니까 뒤집기
        path.Reverse();

        // ##TODO : 임시 -> 오브젝트 설치하기 
        /*
        for(int i = 0; i< path.Count; i++) 
        {
            GameObject temp = Instantiate(_testObject, new Vector3(path[i].y , 2  , path[i].x), Quaternion.identity);
            temp.gameObject.name = i.ToString();
        }
        */

    }

    private int F_Heuristic(int sX , int sY, int dX, int dY) 
    {
        return Math.Abs(dX - sX) + Math.Abs(dY - sY);
    }

    private void F_InitContainer( int startX , int startY ) 
    {
        int indexI = 0;
        int indexJ = 0;
        
        // _mapGrid -> 방문했으면 false
        // visit : 방문 or 접근못하는곳이면 false , 방문 안했으면 true
        // heuristic : 접근가능한 좌표면 max로 
        for (int y = startY - _chunk; y < startY + _chunk; y++) 
        {
            // j 초기화 
            indexJ = 0;

            for (int x = startX - _chunk; x < startX + _chunk; x++)
            {
                //Debug.Log( i + " , " + j + " => " + indexI + " , " +indexJ);

                // 올바르지 않은 좌표면 -> 방문 x
                if ( y < 0 || x < 0 || y >= _mapSize || x >= _mapSize)
                {
                    visit[indexI ,indexJ ] = false;

                    heuristic[indexI , indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // 올바른데 장애물이 있으면 ?
                if (_mapGrid[y,x] == false)
                {
                    visit[indexI, indexJ] = false;

                    heuristic[indexI, indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // 올바르고 장애물도 없으면
                visit[indexI, indexJ]       = true;
                heuristic[indexJ, indexI]   = Int32.MaxValue;
                indexJ++;

            }

            // i 증가 
            indexI++;
        }

        // ##TODO : 임시로 출력해보기
        // 청크 시작 , 끝 

        /*
        for (int y = 0; y < 20; y++) 
        {
            for(int x = 0; x < 20; x++) 
            {
                if (visit[y, x] == false)
                    continue;

                Instantiate(_testObject , new Vector3(startX - _chunk + x , 1 , startY - _chunk + y) , Quaternion.identity);
            }
        }
        */

    }

    private Vector3 F_FindChunckDestination(Vector3 _start, Vector3 _goal) 
    {
        // 시작점에서 raycast
        RaycastHit _hit;

        bool _flag = Physics.Raycast( _start, (_goal - _start).normalized , out _hit , 200f , UnitManager.Instance.chunckBoundary);

        Debug.Log(_flag);

        // 충돌이 감지되면
        if (_flag)
        {
            //Debug.Log(_hit.transform.position.x + " / " + _hit.transform.position.z);
            //GameObject _obj = Instantiate(_testObject, new Vector3(_hit.transform.position.x, 0, _hit.transform.position.z), Quaternion.identity);

            return _hit.transform.position; 
        }

        return Vector3.zero;
    }


}
