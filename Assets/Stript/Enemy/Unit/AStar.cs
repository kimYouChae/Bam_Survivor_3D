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
    private int[] dx = new int[4] { 0,1,0,-1 };
    private int[] dy = new int[4] { 1,0,-1,0 };

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

        //����mapgrid ���
        /*
        for (int y = 0; y < 140; y++) 
        {
            for (int x = 0; x < 140; x++) 
            {
                // �湮������ false
                if (_mapGrid[y,x] == false)
                    continue;

                GameObject _te = Instantiate(_testObject , new Vector3(x,0,y) , Quaternion.identity);
            }
        }
        */

        // ##TODO
        // �ӽ� unit���� �غ��� 
        F_SearchPath(_testUnit.transform.position, new Vector3(2, 1, 3));

    }

    private void Update()
    {
        Debug.DrawRay(new Vector3(20, 1, 45), (new Vector3(2, 1, 3) - new Vector3(20, 1, 45)).normalized * 100f , Color.blue);
    }

    public void F_SearchPath(Vector3 _start, Vector3 _goal) 
    {

        // ûũ(20x20)��ŭ �迭�� �ʱ�ȭ
        F_InitContainer( (int)_start.x, (int)_start.z);

        // ûũ �� ������ ���ϱ� 
        Vector3 desti = F_FindChunckDestination( _start , _goal );

        Debug.Log("������ : " + desti.x +  " , " + desti.z);

        // A*�� ��ã�� 
        F_FindAStarPath( (int)_start.x , (int)_start.z , (int)desti.x , (int)desti.z);

        // �̵���� ���ϱ� 
        F_CheckPath((int)_start.x, (int)_start.z, (int)desti.x, (int)desti.z);
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


        //Debug.Log(MinX + " , " + MinY + " / " + MaxX + " , " + MaxY);

        // �湮ó�� �ߵ��ִ��� ��� (�ε��� ����!@! )
        /*
        for (int y = 0; y < 20; y++) 
        {
            for (int x = 0; x < 20; x++) 
            {
                int nowX = startX - _chunk + x;
                int nowY = startY - _chunk + y; 

                if(nowX < 0 || nowY < 0)
                    continue;

                // �湮�� �� ������ 
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
            for (int i = 0; i < 4; i++) 
            {
                int nx = _currX + dx[i];
                int ny = _currY + dy[i];

                // �ùٸ� ��ǥ�� �ִ��� 
                if (nx < MinX || ny < MinY || nx >= MaxX || ny >= MaxY)
                {
                    //Debug.Log("1���� continew");
                    continue;
                }

                // �湮�ߴ��� (�湮������ false)
                int norX = nx - (startX - _chunk);
                int norY = ny - (startY - _chunk);
                if (visit[norY , norX] == false)
                {
                    //Debug.Log("2���� continue");
                    continue;
                }

                //  g + h�� ����� �޸���ƽ���� ũ��?
                int nowDis = _dis + 1;
                int nowHeuri = F_Heuristic( nx , ny , goalX , goalY);

                //Debug.Log( "(" + norY + " , " + norX + ") "+ "/ ���� : " + (nowDis + nowHeuri).ToString() + " / ����� �޸���ƽ : " + heuristic[norY, norX]);
                if (nowDis + nowHeuri > heuristic[norY, norX])
                {
                    //Debug.Log("3���� continue"); 
                    continue;
                }

                // ���������ϸ�
                // List�� �ֱ� 
                Node _nextNode = new Node(nx, ny, nowDis , nowHeuri);
                _nodes.Add(_nextNode);

                // �湮ó�� 
                visit[norY , norX ] = false;

                // ����ġ(d+h) �ֱ� 
                heuristic[norY , norX] = nowDis + nowHeuri;

                // ������
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

        Debug.Log( "������ ���� : "+ cx + " , " + cy);

        if (pathFinder[cx , cy] == null)
            Debug.LogError("tlqkf");

       


        /*
        while (true)
        {
            Debug.Log("���� ( " + curX + " , " + curY + ")");

            int cx = curX - (startX - _chunk);
            int cy = curY - (startY - _chunk);

            if ( curX == startX && curY == startY)
                break;

            path.Add(new Pos(curY , curX));

            curX = temp.x;
            curY = temp.y;

        }
        */

        // �� ��ġ�� : ������ ����Ʈ�� ���
        path.Add(new Pos(curY , curX));
        // ���� ��->�������� ������ϱ� ������
        path.Reverse();

        // ##TODO : �ӽ� -> ������Ʈ ��ġ�ϱ� 
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
        
        // _mapGrid -> �湮������ false
        // visit : �湮 or ���ٸ��ϴ°��̸� false , �湮 �������� true
        // heuristic : ���ٰ����� ��ǥ�� max�� 
        for (int y = startY - _chunk; y < startY + _chunk; y++) 
        {
            // j �ʱ�ȭ 
            indexJ = 0;

            for (int x = startX - _chunk; x < startX + _chunk; x++)
            {
                //Debug.Log( i + " , " + j + " => " + indexI + " , " +indexJ);

                // �ùٸ��� ���� ��ǥ�� -> �湮 x
                if ( y < 0 || x < 0 || y >= _mapSize || x >= _mapSize)
                {
                    visit[indexI ,indexJ ] = false;

                    heuristic[indexI , indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // �ùٸ��� ��ֹ��� ������ ?
                if (_mapGrid[y,x] == false)
                {
                    visit[indexI, indexJ] = false;

                    heuristic[indexI, indexJ] = 0;

                    indexJ++;
                    continue;
                }

                // �ùٸ��� ��ֹ��� ������
                visit[indexI, indexJ]       = true;
                heuristic[indexJ, indexI]   = Int32.MaxValue;
                indexJ++;

            }

            // i ���� 
            indexI++;
        }

        // ##TODO : �ӽ÷� ����غ���
        // ûũ ���� , �� 

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
        // ���������� raycast
        RaycastHit _hit;

        bool _flag = Physics.Raycast( _start, (_goal - _start).normalized , out _hit , 200f , UnitManager.Instance.chunckBoundary);

        Debug.Log(_flag);

        // �浹�� �����Ǹ�
        if (_flag)
        {
            //Debug.Log(_hit.transform.position.x + " / " + _hit.transform.position.z);
            //GameObject _obj = Instantiate(_testObject, new Vector3(_hit.transform.position.x, 0, _hit.transform.position.z), Quaternion.identity);

            return _hit.transform.position; 
        }

        return Vector3.zero;
    }


}
