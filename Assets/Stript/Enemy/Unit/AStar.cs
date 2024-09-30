using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Jobs;
using UnityEngine;

// �ٸ�Ŭ�������� ���� �� IComparer<T>
// �ش� Ŭ�������� ���� �� IComparable<T>
public class HeuristicComparer : IComparer<Node>
{
    public int Compare(Node x, Node y)
    {
        if (x == null || y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if (x.heuristic < y.heuristic) return -1;
        if (x.heuristic > y.heuristic) return 1;
        return 0;
        // return x.Heuristic.CompareTo(y.heurustic)�� ���� 
    }
}

public class AStar : MonoBehaviour
{
    [SerializeField]
    bool[,] _mapGrid;
    int _mapSize;
    int _chunk = 10;

    [Header("===Container===")]
    private bool[,] visit;
    private int[,] heuristic;

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

    }

    public void F_SearchPath(int startX, int startY , int goalX, int goalY) 
    {
        // ûũ(20x20)��ŭ �迭�� �ʱ�ȭ
        F_InitContainer(startX , startY);

        // ûũ �� ������ ���ϱ� 
        var chunckDesti = F_FindChunckDestination(startX , startY , goalX , goalY);

        // A*�� ��ã�� 
        F_FindAStarPath(startX, startY, chunckDesti.Item1 , chunckDesti.Item2);

    }

    private void F_FindAStarPath( int startX, int startY, int goalX, int goalY) 
    {
        // node����Ʈ�� �߰� 
        Node startNode = new Node( startX, startY, 0, F_Heuristic(startX, startY, goalX, goalY));
        _nodes.Add(startNode);

        // �湮ó�� 
        visit[0, 0] = true;

        // �޸���ƽ ���� 
        heuristic[0, 0] = startNode.heuristic;
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

    private (int, int) F_FindChunckDestination(int sX, int sY , int dX , int dY) 
    {
        // grid ������ �� ����� ����
        int chunckMaxX = Math.Min(sX + _chunk , _mapSize);
        int chunckMaxY = Math.Min(sY + _chunk, _mapSize);

        // ûũ �� ������ ã��
        int chunckDestX = Math.Min(dX, chunckMaxX);
        int chunckDestY = Math.Min(dY, chunckMaxY);

        return (chunckDestX, chunckDestY);
    }

}
