﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleLang
{
    public interface Visitor
    {
        void Visit(IdNode id);
        void Visit(IntNumNode num);
        void Visit(BoolNode binop);
        void Visit(BinExprNode binop);
        void Visit(AssignNode assNode);
        void Visit(CycleNode cycNode);
        void Visit(BlockNode blNode);
        void Visit(IfNode ifNode);
        void Visit(ForNode forNode);
        void Visit(RepUntNode ruNode); 
        void Visit(WhileNode whNode); 
    }

    public class PrettyPrintVisitor : Visitor
    {
        public string Text = "";
        private int Indent = 0;

        private string IndentStr() { return new string(' ', Indent); }
        private void IndentPlus() { Indent += 2; }
        private void IndentMinus() { Indent -= 2; }
        public void Visit(IdNode id) { Text += id.Name; }
        public void Visit(IntNumNode num) { Text += num.Num.ToString(); }
        public void Visit(BoolNode binop) { Text += binop.Bool.ToString(); }

        public void Visit(BinExprNode binop)
        {
            Text += "(";
            binop.ExprLeft.Accept(this);
            Text += " " + binop.BinSign + " ";
            binop.ExprRight.Accept(this);
            Text += ")";
        }

        public void Visit(AssignNode a)
        {
            Text += IndentStr();
            a.Id.Accept(this);
            Text += " := ";
            a.Expr.Accept(this);
        }
        public void Visit(CycleNode c)
        {
            Text += IndentStr() + "cycle ";
            c.Expr.Accept(this);
            Text += Environment.NewLine;
            c.Stat.Accept(this);
        }
        public void Visit(BlockNode bl)
        {
            Text += IndentStr() + "begin" + Environment.NewLine;
            IndentPlus();

            var Count = bl.StList.Count;

            if (Count > 0)
                bl.StList[0].Accept(this);
            for (var i = 1; i < Count; i++)
            {
                Text += ';';
                if (!(bl.StList[i] is /*EmptyNode /*what is it?*/ StatementNode))
                    Text += Environment.NewLine;
                bl.StList[i].Accept(this);
            }
            IndentMinus();
            Text += Environment.NewLine + IndentStr() + "end";
        }
        public void Visit(IfNode bl)
        {
            throw new NotImplementedException();
        }

        public void Visit(ForNode forNode)
        {
            throw new NotImplementedException();
        }

        public void Visit(RepUntNode ruNode)
        {
            throw new NotImplementedException();
        }

        public void Visit(WhileNode whNode)
        {
            throw new NotImplementedException();
        }
    }

}