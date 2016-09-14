using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };
    public enum BinSign { LS, GT, LE, GE, EQ, NE };

    public class Node    
    {
    }

    public class ExprNode : Node 
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public IdNode(string name) { Name = name; }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
    }

    public class BoolNode : ExprNode
    {
        public bool Bool { get; set; }
        public BoolNode(bool flag) { Bool = flag; }
    }

    public class StatementNode : Node 
    {
    }

    public class AssignNode : StatementNode
    {
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();

        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }

        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
    }

    public class ElseNode : StatementNode 
    {
        public StatementNode Stat { get; set; }

        public ElseNode(StatementNode stat) 
        {
            Stat = stat;
        }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public StatementNode TrueBranch { get; set; }
        public ElseNode ElseStatement { get; set; }

        public IfNode(ExprNode condition, StatementNode trueBranch) 
        {
            Condition = condition;
            TrueBranch = trueBranch;
        }

        public IfNode(ExprNode condition, StatementNode trueBranch, ElseNode elseStatement)
            : this (condition, trueBranch)
        {
            ElseStatement = elseStatement;
        }
    }

    public class ForNode : StatementNode 
    {
        public AssignNode LeftLimit { get; set; }
        public ExprNode RightLimit { get; set; }
        public StatementNode DoStat { get; set; }

        public ForNode(AssignNode leftLimit, ExprNode rightLimit, StatementNode doStat ) 
        {
            LeftLimit = leftLimit;
            RightLimit = rightLimit;
            DoStat = doStat;
        }

    }

    public class BinExprNode : ExprNode 
    {
        public BinExprNode BinExpr  { get; set; }
        public BinSign BinSign  { get; set; }
        public ExprNode Expr { get; set; }

        public BinExprNode(BinExprNode binExpr, BinSign binSign, ExprNode expr) 
        {
            BinExpr = binExpr;
            BinSign = binSign;
            Expr = expr;
        }
    }

    public class RepUntNode : StatementNode 
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BinExprNode UntilExpr { get; set; }

        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }

        public RepUntNode(StatementNode stat, BinExprNode untilExpr)
        {
            UntilExpr = untilExpr;
            Add(stat);
        }
    }

    public class WhileNode : StatementNode
    {
        public BinExprNode Condition { get; set; }
        public StatementNode Stat { get; set; }

        public WhileNode(BinExprNode condition, StatementNode stat) 
        {
            Condition = condition;
            Stat = stat;
        }
    }
}
