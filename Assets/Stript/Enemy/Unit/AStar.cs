using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Jobs;
using UnityEngine;

// 다른클래스에서 비교할 땐 IComparer<T>
// 해당 클래스에서 비교할 땐 IComparable<T>
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
        // return x.Heuristic.CompareTo(y.heurustic)과 같음 
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

    }

    public void F_SearchPath(int startX, int startY , int goalX, int goalY) 
    {
        // 청크(20x20)만큼 배열들 초기화
        F_InitContainer(startX , startY);

        // 청크 내 도착지 정하기 
        var chunckDesti = F_FindChunckDestination(startX , startY , goalX , goalY);

        // A*로 길찾기 
        F_FindAStarPath(startX, startY, chunckDesti.Item1 , chunckDesti.Item2);

    }

    private void F_FindAStarPath( int startX, int startY, int goalX, int goalY) 
    {
        // node리스트에 추가 
        Node startNode = new Node( startX, startY, 0, F_Heuristic(startX, startY, goalX, goalY));
        _nodes.Add(startNode);

        // 방문처리 
        visit[0, 0] = true;

        // 휴리스틱 설정 
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

    private (int, int) F_FindChunckDestination(int sX, int sY , int dX , int dY) 
    {
        // grid 범위를 안 벗어나게 설정
        int chunckMaxX = Math.Min(sX + _chunk , _mapSize);
        int chunckMaxY = Math.Min(sY + _chunk, _mapSize);

        // 청크 내 도착지 찾기
        int chunckDestX = Math.Min(dX, chunckMaxX);
        int chunckDestY = Math.Min(dY, chunckMaxY);

        return (chunckDestX, chunckDestY);
    }

}
