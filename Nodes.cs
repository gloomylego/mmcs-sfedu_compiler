using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };
    public enum BinSign { LS, GT, LE, GE, EQ, NE };

    public abstract class Node
    {
        public abstract void Accept(Visitor v);
    }

    public abstract class ExprNode : Node 
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }

        public IdNode(string name) { Name = name; }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }

        public IntNumNode(int num) { Num = num; }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class BoolNode : ExprNode
    {
        public bool Bool { get; set; }

        public BoolNode(bool flag) { Bool = flag; }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class BinaryNode : ExprNode
    {
        public ExprNode left { get; set; }
        public ExprNode right { get; set; }
        public string operation { get; set; }

        public BinaryNode(ExprNode lhs, ExprNode rhs, string op)
        {
            left = lhs;
            right = rhs;
            operation = op;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    // Do we really need this class?
    public class BinExprNode : ExprNode
    {
        public BinExprNode BinExpr { get; set; }
        public BinSign BinSign { get; set; }
        public ExprNode Expr { get; set; }

        public BinExprNode(BinExprNode binExpr, BinSign binSign, ExprNode expr)
        {
            BinExpr = binExpr;
            BinSign = binSign;
            Expr = expr;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public abstract class StatementNode : Node
    { }

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

        public override void Accept(Visitor v) { v.Visit(this); }
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

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();

        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }

        public BlockNode(List<StatementNode> statList)
        {
            StList = statList;
        }

        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }

        public override void Accept(Visitor v) { v.Visit(this); }
    }

    public class IfNode : StatementNode
    {
        public ExprNode Condition { get; set; }
        public StatementNode TrueBranch { get; set; }
        public StatementNode ElseStatement { get; set; }

        public IfNode(ExprNode condition, StatementNode trueBranch) 
        {
            Condition = condition;
            TrueBranch = trueBranch;
            ElseStatement = null;
        }

        public IfNode(ExprNode condition, StatementNode trueBranch, StatementNode elseStatement)
            : this (condition, trueBranch)
        {
            ElseStatement = elseStatement;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
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

        public override void Accept(Visitor v) { v.Visit(this); }
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

        public RepUntNode(List<StatementNode> stList, BinExprNode untilExpr)
        {
            StList = stList;
            UntilExpr = untilExpr;
        }

        public override void Accept(Visitor v) { v.Visit(this); }
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

        public override void Accept(Visitor v) { v.Visit(this); }
    }
}
