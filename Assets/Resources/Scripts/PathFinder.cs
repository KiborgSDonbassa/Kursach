using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    private List<GamePanel> CheckedGamePanels = new List<GamePanel>();
    private List<GamePanel> WaitingGamePanels = new List<GamePanel>();
    public LayerMask LayerToBlock;
    
    public List<Vector2> GetPath(Vector2 target)
    {
        CheckedGamePanels = new List<GamePanel>();
        WaitingGamePanels = new List<GamePanel>();

        Vector2 StartPosition = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
        Vector2 TargetPosition = new Vector2(Mathf.Round(target.x), Mathf.Round(target.y));
        
        if(StartPosition == TargetPosition) return new List<Vector2>();

        GamePanel startGamePanel = new GamePanel(0, StartPosition, TargetPosition, null);
        CheckedGamePanels.Add(startGamePanel);
        WaitingGamePanels.AddRange(GetNeighbourGamePanels(startGamePanel));
        if (Physics2D.OverlapCircle(TargetPosition, 0.1f, LayerToBlock))
        {
            TargetPosition = SearchNearFreeGamePanel(TargetPosition);
        }
        while(WaitingGamePanels.Count > 0)
        {
            GamePanel nodeToCheck = WaitingGamePanels.Where(x => x.F == WaitingGamePanels.Min(y => y.F)).FirstOrDefault();

            if (nodeToCheck.Position == TargetPosition)
            {
                return CalculatePathFromGamePanel(nodeToCheck);
            }
            bool walkable = !Physics2D.OverlapCircle(nodeToCheck.Position, 0.1f, LayerToBlock);
            if (!walkable)
            {
                WaitingGamePanels.Remove(nodeToCheck);
                CheckedGamePanels.Add(nodeToCheck);
            }
            else if (walkable)
            {
                WaitingGamePanels.Remove(nodeToCheck);
                if (!CheckedGamePanels.Where(x => x.Position == nodeToCheck.Position).Any())
                {
                    CheckedGamePanels.Add(nodeToCheck);
                    WaitingGamePanels.AddRange(GetNeighbourGamePanels(nodeToCheck));
                }
            }
        }
        return null;
    }
    public List<Vector2> CalculatePathFromGamePanel(GamePanel node)
    {
        List<Vector2> path = new List<Vector2>();
        GamePanel currentGamePanel = node;

        while(currentGamePanel.PreviousGamePanel != null)
        {
            path.Add(new Vector2(currentGamePanel.Position.x, currentGamePanel.Position.y));
            currentGamePanel = currentGamePanel.PreviousGamePanel;
        }

        return path;
    }
    private List<GamePanel> GetNeighbourGamePanels (GamePanel node)
    {
        List<GamePanel> Neighbours = new List<GamePanel>();
        Neighbours.Add(new GamePanel(node.startDistance + 1, new Vector2(
            node.Position.x-1, node.Position.y), 
            node.TargetPosition, 
            node));
        Neighbours.Add(new GamePanel(node.startDistance + 1, new Vector2(
            node.Position.x+1, node.Position.y),
            node.TargetPosition,
            node));
        Neighbours.Add(new GamePanel(node.startDistance + 1, new Vector2(
            node.Position.x, node.Position.y-1),
            node.TargetPosition,
            node));
        Neighbours.Add(new GamePanel(node.startDistance + 1, new Vector2(
            node.Position.x, node.Position.y+1),
            node.TargetPosition,
            node));

        Neighbours.Add(new GamePanel(node.startDistance + 1.4f, new Vector2(
            node.Position.x + 1, node.Position.y + 1),
            node.TargetPosition,
            node));
        Neighbours.Add(new GamePanel(node.startDistance + 1.4f, new Vector2(
            node.Position.x - 1, node.Position.y - 1),
            node.TargetPosition,
            node));
        Neighbours.Add(new GamePanel(node.startDistance + 1.4f, new Vector2(
            node.Position.x + 1, node.Position.y - 1),
            node.TargetPosition,
            node));
        Neighbours.Add(new GamePanel(node.startDistance + 1.4f, new Vector2(
            node.Position.x - 1, node.Position.y + 1),
            node.TargetPosition,
            node));
        
        
        return Neighbours;
    }
    public Vector2 SearchNearFreeGamePanel(Vector2 targetPos)
    {
        List<Vector2> CheckingGamePanels = new List<Vector2>();
        List<Vector2> BlackList = new List<Vector2>();
        targetPos = new Vector2((float)Math.Round(targetPos.x), (float)Math.Round(targetPos.y));
        CheckingGamePanels.Add(targetPos);
        Vector2 returendValue;
        int i = 0;
        while (i < 500)
        {
            returendValue = CheckingGamePanels.FirstOrDefault();
            if (!Physics2D.OverlapCircle(returendValue,0.1f,LayerToBlock) && !BlackList.Contains(returendValue))
            {
                return returendValue;
            }
            else
            {
                foreach (Vector2 aaa in GetNeighbourPos(returendValue))
                {
                    if (!BlackList.Contains(aaa)) CheckingGamePanels.Add(aaa);
                }
                CheckingGamePanels.Remove(returendValue);
                BlackList.Add(returendValue);
            }

            i++;
        }
        return Hero.Player.transform.position;
    }
    private List<Vector2> GetNeighbourPos(Vector2 targetPos)
    {
        List<Vector2> a = new List<Vector2>();
        a.Add(targetPos + new Vector2(0,1));
        a.Add(targetPos + new Vector2(0,-1));
        a.Add(targetPos + new Vector2(1,0));
        a.Add(targetPos + new Vector2(-1,0));
        return a;
    }
}

public class GamePanel 
{
    public Vector2 Position;
    public Vector2 TargetPosition;
    public GamePanel PreviousGamePanel;
    public float F;
    public float startDistance;
    public float targetDistance;

    public GamePanel(float g, Vector2 _Position, Vector2 targetPosition, GamePanel previousGamePanel)
    {
        Position = _Position;
        TargetPosition = targetPosition;
        PreviousGamePanel = previousGamePanel;
        startDistance = g;
        targetDistance = (int)Mathf.Abs(targetPosition.x - Position.x) + (int)Mathf.Abs(targetPosition.y - Position.y);
        F = startDistance + targetDistance;
    }
}