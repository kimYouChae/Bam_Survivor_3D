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
    private int[] dx = new int[8] { 0, 1, 1, 1, 0, -1, -1, -1 };
    private int[] dy = new int[8] { -1, -1, 0, 1, 1, 1, 0, -1 };

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

        // A*로 길찾기 
        F_FindAStarPath( (int)_start.x , (int)_start.z , (int)desti.x , (int)desti.z);

        // 이동경로 정하기 
        //F_CheckPath((int)_start.x, (int)_start.y, (int)desti.x, (int)desti.y);
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

        Debug.Log(MinX + " , " + MinY + " / " + MaxX + " , " + MaxY);

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

            Debug.Log("현재노드 " + _currX + " , " + _currY);
             
            //도착하면?
            if (_currX == goalX && _currY == goalY)
                break;

            // 8방향으로
            for (int i = 0; i < 8; i++) 
            {
                int nx = _currX + dx[i];
                int ny = _currY + dy[i];

                // 올바른 좌표에 있는지 
                if (nx < MinX || ny < MinY || nx >= MaxX || ny >= MaxY)
                {
                    Debug.Log("1에서 continew");
                    continue;
                }

                // 방문했는지 (장애물도 true)
                int norX = nx - (startX - _chunk);
                int norY = ny - (startY - _chunk);
                if (visit[norX, norY])
                {
                    Debug.Log("2에서 continue");
                    continue;
                }

                //  g + h가 저장된 휴리스틱보다 크면?
                int nowDis = _dis + 1;
                int nowHeuri = F_Heuristic( nx , ny , goalX , goalY);
                if (nowDis + nowHeuri > heuristic[norX, norY])
                {
                    Debug.Log("3에서 continue"); 
                    continue;
                }

                // 조건충족하면
                // List에 넣기 
                Node _nextNode = new Node(nx, ny, nowDis , nowHeuri);
                _nodes.Add(_nextNode);

                // 방문처리 
                visit[norX, norY] = true;

                // 가중치(d+h) 넣기 
                heuristic[norX, norY] = nowDis + nowHeuri;

                // 경로잡기
                pathFinder[norX, norY] 
                    = new Pos( _currX - (startX - _chunk) , _currY - (startX - _chunk));
            }

        }
    }

    private void F_CheckPath(int startX, int startY ,int goalX, int goalY) 
    {
        int curX = goalX;
        int curY = goalY;

        while (true)
        {
            int cx = curX - (startX - _chunk);
            int cy = curY - (startY - _chunk);

            if ( curX == goalX && curY == goalY)
                break;

            path.Add(new Pos(curX, curY));

            Pos temp = pathFinder[curX, curY];
            curX = temp.x;
            curY = temp.y;
        }

        // 맨 마치막 : 시작점 리스트에 담기
        path.Add(new Pos(curX, curY));
        // 현재 끝->시작으로 담았으니까 뒤집기
        path.Reverse();

        // ##TODO : 임시 -> 오브젝트 설치하기 
        for(int i = 0; i< path.Count; i++) 
        {
            GameObject temp = Instantiate(_testObject, new Vector3(path[i].x + startX, 0 ,path[i].y + startY), Quaternion.identity);
            temp.gameObject.name = i.ToString();
        }
    }

    private int F_Heuristic(int sX , int sY, int dX, int dY) 
    {
        return Math.Abs(dX - sX) + Math.Abs(dY - sY);
    }

    private void F_InitContainer( int startX , int startY ) 
    {
        int indexI = 0;
        int indexJ = 0;

        for (int i = startY - _chunk; i < startY + _chunk; i++) 
        {
            // j 초기화 
            indexJ = 0;

            for (int j = startX - _chunk; j < startX + _chunk; j++)
            {
                //Debug.Log( i + " , " + j + " => " + indexI + " , " +indexJ);

                // 올바르지 않은 좌표면 -> 방문 x
                if (!F_ValidePosition(i, j))
                {
                    visit[indexI, indexJ] = true;

                    heuristic[indexI, indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // 올바른데 장애물이 있으면 ?
                if (!_mapGrid[i, j])
                {
                    visit[indexI, indexJ] = true;

                    heuristic[indexI, indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // 올바르고 장애물도 없으면
                visit[indexI, indexJ]       = false;
                heuristic[indexI, indexJ]   = Int32.MaxValue;
                indexJ++;

            }

            // i 증가 
            indexI++;
        }

        // ##TODO : 임시로 출력해보기
        // 청크 시작 , 끝 
        int _cXS = startX - _chunk;
        int _cYS = startY - _chunk;

        for (int i = 0; i < 20; i++) 
        {
            for(int j = 0; j < 20; j++) 
            {
                if (visit[j, i] == true)
                    continue;

                Instantiate(_testObject , new Vector3(_cYS + j, 1 , _cXS + i) , Quaternion.identity);
            }
        }
    }

    private bool F_ValidePosition( int x , int y ) 
    {
        if (x < 0 || y < 0 || x >= _chunk || y >= _chunk)
            return false;

        return true;
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
