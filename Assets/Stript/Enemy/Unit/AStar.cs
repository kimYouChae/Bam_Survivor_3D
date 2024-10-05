using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// �ٸ�Ŭ�������� ���� �� IComparer<T>
// �ش� Ŭ�������� ���� �� IComparable<T>
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
        // return x.Heuristic.CompareTo(y.heurustic)�� ���� 
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

    // �켱����ť ��ſ� ����� List
    [Header("===Priority List===")]
    List<Node> _nodes;
    //PriorityQueue<Node, int> _priority = new PriorityQueue<Node, int>();
    //  ��ǥ , ������� �Ÿ� g + �޸���ƽ h (���� ������ g+h )

    private void Start()
    {
        // ��������
        _mapGrid    = MapManager.instance.mapGrid;

        _mapSize    = MapManager.instance.mapSize;

        visit       = new bool[_chunk * 2, _chunk * 2];
        heuristic   = new int[_chunk * 2, _chunk * 2];

        _nodes      = new List<Node>();

        pathFinder = new Pos[_chunk * 2, _chunk * 2];
        path = new List<Pos>();

        Debug.Log("�׽�Ʈ ���� ��ġ " +_testUnit.transform.position.x + " / " + _testUnit.transform.position.z);
        
        // ##TODO
        // �ӽ� unit���� �غ��� 
        F_SearchPath((int)_testUnit.transform.position.x , (int)_testUnit.transform.position.z
            , 2,3);

    }

    public void F_SearchPath(int startX, int startY , int goalX, int goalY) 
    {
        // ûũ(20x20)��ŭ �迭�� �ʱ�ȭ
        F_InitContainer(startX , startY);

        // ûũ �� ������ ���ϱ� 
        var desti = F_FindChunckDestination(startX, startY, goalX, goalY);

        Debug.Log("unit ���� : " + startX + " / " + startY);
        Debug.Log("ûũ �� ������" + desti.Item1 + " / " + desti.Item2);

        // A*�� ��ã�� 
        F_FindAStarPath(startX, startY, (int)desti.Item1 , (int)desti.Item2);

        // �̵���� ���ϱ� 
        F_CheckPath(startX , startY , goalX, goalY);
    }

    private void F_FindAStarPath( int startX, int startY, int goalX, int goalY) 
    {
        // node����Ʈ�� �߰� 
        Node startNode = new Node( startX, startY, 0, F_Heuristic(startX, startY, goalX, goalY));
        _nodes.Add(startNode);

        // �湮ó�� 
        visit[0, 0] = true;

        // �޸���ƽ ���� 
        heuristic[0, 0] = startNode.weight;

        // ���� ��ǥ�� ���� max
        int MinX = Math.Max(startX - _chunk, 0);
        int MaxX = Math.Min(startX + _chunk, _mapSize);
        int MinY = Math.Max(startY - _chunk, 0);
        int MaxY = Math.Min(startY + _chunk, _mapSize);

        while (true) 
        {
            if (_nodes.Count == 0)
                break;

            // ���� ��� 
            _nodes.Sort(new HeuristicComparer());  // ���� ���� �������� sort �� 
            Node _curr = _nodes[0];
            _nodes.RemoveAt(0);

            int _currX = _curr.x; 
            int _currY = _curr.y;
            int _dis = _curr.distance; 
            int _heuri = _curr.weight;

            //�����ϸ�?
            if (_currX == goalX && _currY == goalY)
                break;

            // 8��������
            for (int i = 0; i < 8; i++) 
            {
                int nx = _currX + dx[i];
                int ny = _currY + dy[i];

                // �ùٸ� ��ǥ�� �ִ��� 
                if (nx < MinX || ny < MinY || nx >= MaxX || ny >= MaxY)
                    continue;

                // �湮�ߴ��� (��ֹ��� true)
                int norX = nx - (startX - _chunk);
                int norY = ny - (startY - _chunk);
                if (visit[norX, norY])
                    continue;

                //  g + h�� ����� �޸���ƽ���� ũ��?
                int nowDis = _dis + 1;
                int nowHeuri = F_Heuristic( nx , ny , goalX , goalY);
                if (nowDis + nowHeuri > heuristic[norX, norY])
                    continue;

                // ���������ϸ�
                // List�� �ֱ� 
                Node _nextNode = new Node(nx, ny, nowDis , nowHeuri);
                _nodes.Add(_nextNode);

                // �湮ó�� 
                visit[norX, norY] = true;

                // ����ġ(d+h) �ֱ� 
                heuristic[norX, norY] = nowDis + nowHeuri;

                // ������
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

        // �� ��ġ�� : ������ ����Ʈ�� ���
        path.Add(new Pos(curX, curY));
        // ���� ��->�������� ������ϱ� ������
        path.Reverse();

        // ##TODO : �ӽ� -> ������Ʈ ��ġ�ϱ� 
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
            // j �ʱ�ȭ 
            indexJ = 0;

            for (int j = startX - _chunk; j < startX + _chunk; j++)
            {
                //Debug.Log( i + " , " + j + " => " + indexI + " , " +indexJ);

                // �ùٸ��� ���� ��ǥ�� -> �湮 x
                if (!F_ValidePosition(i, j))
                {
                    visit[indexI, indexJ] = true;

                    heuristic[indexI, indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // �ùٸ��� ��ֹ��� ������ ?
                if (!_mapGrid[i, j])
                {
                    visit[indexI, indexJ] = true;

                    heuristic[indexI, indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // �ùٸ��� ��ֹ��� ������
                visit[indexI, indexJ]       = false;
                heuristic[indexI, indexJ]   = Int32.MaxValue;
                indexJ++;

            }

            // i ���� 
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
        // ������ �� �־����
        int chunkDestiX = Int32.MaxValue;
        int chunkDestiY = Int32.MaxValue;

        // start�� goal�� ������ ������ ������ ���ϱ�
        // y = mx + a 
        // ���������� ���� 
        double maininClination = Math.Round( ((double)goalY - (double)startY) / ((double)goalX - (double)startX) , 2);
        double mainA = Math.Round( ( -1.0 * (maininClination * (double)startX)) + (double)startY , 2);

        Debug.Log("���� + " + maininClination);
        Debug.Log("A : " + mainA);

        // ��� ûũ line�� ���� ������ ������ ���ϱ�
        // -> �� �� start-goal �����̶� ������ �ִ��� �˻� ?

        int chunckMinX = Math.Max(startX - 10, 0);
        int chunckMaxX = Math.Min(startX + 10, _mapSize);
        int chunckMinY = Math.Max(startY - 10, 0);
        int chunckMaxY = Math.Min(startY + 10, _mapSize);

        Debug.Log("( " + chunckMinX + " , " + chunckMinY + ") ���� ( " + chunckMaxX + " , " + chunckMaxY + " ) ");

        // goal�� ���ʿ� ������ 
        if (goalY > startY && goalX >= chunckMinX && goalX <=  chunckMaxX)
        {
            chunkDestiX = chunckMaxY; 
            chunkDestiY = (int)mainA;     
        }
        // �Ʒ���
        else if (goalY < startY && goalX >= chunckMinX && goalX <= chunckMaxX)
        {
            chunkDestiX = chunckMinY; 
            chunkDestiY = (int)mainA;
        }
        // ������
        else if (goalX > startX)
        {
            chunkDestiX = chunckMaxX;
            chunkDestiY = (int)(maininClination * startX * -1) * chunkDestiX + startY;
        }
        // ���� 
        else if(goalX < startX)
        {
            Debug.Log("!!!!!!!!!!!!!!!�����");
            chunkDestiX = chunckMinX;
            chunkDestiY = (int)(maininClination * chunkDestiX + mainA);
        }

        return (chunkDestiX, chunkDestiY);

    }


}
