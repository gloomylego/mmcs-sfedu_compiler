using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{

    public class Node
    {}

    public class ExprNode : Node
    {}

    public class StatementNode : Node
    {}

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

    }
}
