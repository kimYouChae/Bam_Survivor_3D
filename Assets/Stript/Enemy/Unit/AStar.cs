using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

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
        F_SearchPath((int)_testUnit.transform.position.x , (int)_testUnit.transform.position.z
            , 2,3);

    }

    public void F_SearchPath(int startX, int startY , int goalX, int goalY) 
    {
        // 청크(20x20)만큼 배열들 초기화
        F_InitContainer(startX , startY);

        // 청크 내 도착지 정하기 
        var desti = F_FindChunckDestination(startX, startY, goalX, goalY);

        Debug.Log("unit 시작 : " + startX + " / " + startY);
        Debug.Log("청크 내 도착지" + desti.Item1 + " / " + desti.Item2);

        // A*로 길찾기 
        F_FindAStarPath(startX, startY, (int)desti.Item1 , (int)desti.Item2);

        // 이동경로 정하기 
        F_CheckPath(startX , startY , goalX, goalY);
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
            for (int i = 0; i < 8; i++) 
            {
                int nx = _currX + dx[i];
                int ny = _currY + dy[i];

                // 올바른 좌표에 있는지 
                if (nx < MinX || ny < MinY || nx >= MaxX || ny >= MaxY)
                    continue;

                // 방문했는지 (장애물도 true)
                int norX = nx - (startX - _chunk);
                int norY = ny - (startY - _chunk);
                if (visit[norX, norY])
                    continue;

                //  g + h가 저장된 휴리스틱보다 크면?
                int nowDis = _dis + 1;
                int nowHeuri = F_Heuristic( nx , ny , goalX , goalY);
                if (nowDis + nowHeuri > heuristic[norX, norY])
                    continue;

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
    }

    private bool F_ValidePosition( int x , int y ) 
    {
        if (x < 0 || y < 0 || x >= _chunk || y >= _chunk)
            return false;

        return true;
    }

    private (int,int) F_FindChunckDestination(int startX, int startY, int goalX, int goalY) 
    {
        // 임의의 값 넣어놓기
        int chunkDestiX = Int32.MaxValue;
        int chunkDestiY = Int32.MaxValue;

        // start와 goal을 지나는 직선의 방정식 구하기
        // y = mx + a 
        // 두점사이의 기울기 
        double maininClination = Math.Round( ((double)goalY - (double)startY) / ((double)goalX - (double)startX) , 2);
        double mainA = Math.Round( ( -1.0 * (maininClination * (double)startX)) + (double)startY , 2);

        Debug.Log("기울기 + " + maininClination);
        Debug.Log("A : " + mainA);

        // 모든 청크 line에 대한 직선의 방정식 구하기
        // -> 그 후 start-goal 직선이랑 교점이 있는지 검사 ?

        int chunckMinX = Math.Max(startX - 10, 0);
        int chunckMaxX = Math.Min(startX + 10, _mapSize);
        int chunckMinY = Math.Max(startY - 10, 0);
        int chunckMaxY = Math.Min(startY + 10, _mapSize);

        Debug.Log("( " + chunckMinX + " , " + chunckMinY + ") 부터 ( " + chunckMaxX + " , " + chunckMaxY + " ) ");

        // goal이 위쪽에 있으면 
        if (goalY > startY && goalX >= chunckMinX && goalX <=  chunckMaxX)
        {
            chunkDestiX = chunckMaxY; 
            chunkDestiY = (int)mainA;     
        }
        // 아래쪽
        else if (goalY < startY && goalX >= chunckMinX && goalX <= chunckMaxX)
        {
            chunkDestiX = chunckMinY; 
            chunkDestiY = (int)mainA;
        }
        // 오른쪽
        else if (goalX > startX)
        {
            chunkDestiX = chunckMaxX;
            chunkDestiY = (int)(maininClination * startX * -1) * chunkDestiX + startY;
        }
        // 왼쪽 
        else if(goalX < startX)
        {
            Debug.Log("!!!!!!!!!!!!!!!현재왼");
            chunkDestiX = chunckMinX;
            chunkDestiY = (int)(maininClination * chunkDestiX + mainA);
        }

        return (chunkDestiX, chunkDestiY);

    }


}
