using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaker : MonoBehaviour
{
    bool[,] wall = new bool[30, 30];
    bool[,] passed = new bool[30, 30];
    string[] s = {"...######.....######..........",
"##....#######.######.####.####",
"##.##...........####.####.....",
"##..#####.##########.#########",
"###..####.########...#####....",
"####.####.......##.####....###",
"........#######....####.######",
".######.######..#######..#####",
".######.....##.#########.#####",
".######.###.########.....#####",
"..#####..##....#####.#########",
"#..#####.#####.......#########",
"##.....#############.....#####",
"######.......##########.######",
"######.#####.....######..#####",
"######.#########...#####...###",
"####......########.#######.###",
"####.####.####.....#####......",
".....##.....#########....####.",
".######.###.##....######.####.",
"......#####....########...####",
".#########..####......########",
"....######.##########..#####..",
"###..#####.......#####.######.",
"####....#..#####..#....#####..",
"###..#####.....####.######...#",
"####.....#####......####...###",
"########.....####.####...#####",
"############..###......#######",
"#####################........."
};

    struct Point
    {
        public int x, y;
    };
    int sx=0, sy=0;
    int gx=29, gy=29;
    int[] dx = { 1, 0, -1, 0 };
    int[] dy = { 0, -1, 0, 1 };

    Stack<Point> stk = new Stack<Point>();
    int interval = 0;

    bool checkWall(int x, int y)
    {
        int cnt = 0;
        for(int i=0; i<4; i++)
        {
            int nx = x + dx[i], ny = y + dy[i];
            if (nx < 0 || 29 < nx || ny < 0 || 29 < ny) cnt++;
            else if(wall[nx, ny] && (x!=sx || y!=sy) && (x!=gx || y!=gy)){
                cnt++;
            }
        }
        return (cnt >= 3);
    }



    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<30; i++)
        {
            for(int j=0; j<30; j++)
            {
                wall[i,j] = passed[i,j] = (s[i][j] == '#');
            }
        }
        Point o;
        o.x = sx;
        o.y = sy;
        stk.Push(o);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<30; i++)
        {
            for(int j=0; j<30; j++)
            {
                if (passed[i, j] && checkWall(i, j)) wall[i, j] = true;

                GameObject wallTarget = GameObject.Find($"InnerWall_{i}_{j}");
                if (wall[i,j] && wallTarget.transform.position.y < 1f)
                {
                    wallTarget.transform.Translate(0f, 0.1f, 0f);
                }
                
                GameObject passedTarget = GameObject.Find($"PassedRoute_{i}_{j}");
                if(passed[i, j] && passedTarget.transform.position.y < 0.2f)
                {
                    passedTarget.transform.Translate(0f, 0.2f, 0f);
                }
            }
        }



        if (interval <= 20)
        {
            interval++;
        }
        else if(stk.Count>0)
        {
            Point p = stk.Pop();
            if (p.x == gx && p.y == gy) Debug.Log("Reached!") ;
            passed[p.x, p.y] = true;
            for (int i = 0; i < 4; i++)
            {
                int nx = p.x + dx[i], ny = p.y + dy[i];
                if (nx < 0 || 29 < nx || ny < 0 || 29 < ny) continue;
                if (!wall[nx, ny] && !passed[nx, ny])
                {
                    Point np;
                    np.x = nx;
                    np.y = ny;
                    stk.Push(np);
                }
            }
        }
    }
}
