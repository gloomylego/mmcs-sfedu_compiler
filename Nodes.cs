using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

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
        public override void Accept(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }
        public override void Accept(Visitor v)
        {
            v.Visit(this);
        }
    }

    public class BoolNode : ExprNode
    {
        public bool Bool { get; set; }
        public BoolNode(bool flag) { Bool = flag; }
        public override void Accept(Visitor v)
        {
            v.Visit(this);
        }
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
        public override void Accept(Visitor v)
        {
            left.Accept(v);
            right.Accept(v);
            v.Visit(this);
        }
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
        public override void Accept(Visitor v)
        {
            v.Visit(this);
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
        public override void Accept(Visitor v)
        {
            v.Visit(this);
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
        public override void Accept(Visitor v)
        {
            v.Visit(this);
            //foreach(StatementNode sNode in StList)
            //    sNode.Accept(v);
            
        }
    }
}
