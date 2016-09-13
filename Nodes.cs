using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    abstract public class Node
    {
        abstract public int getValue();   
    };

    public class ExprNode : Node
    {
        public ExprNode(int val)
        {
            value = val;
        }
        public ExprNode(string op, Node l, Node r)
        {
            value = null;
            operation = op;
            left = l;
            right = r;
        }

        public override int getValue()
        {
            return value != null ? value.Value : Implementation.calculate(left, operation, right);
        }

        int? value = null;
        string operation = null;
        private Node left = null;
        private Node right = null;

        
    }

    public class StatementNode : Node
    {
        public override int getValue()
        {
            throw new NotImplementedException();
        }
    }

    public class BlockNode : Node
    {
        public BlockNode(StatementNode statement)
        {
            // TODO
        }

        public void Add(StatementNode statement)
        {
            // TODO
        }

        public override int getValue()
        {
            throw new NotImplementedException();
        }
    }
}
