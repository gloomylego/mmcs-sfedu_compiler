﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace SimpleLang
{

    abstract public class InstructionTerm
    {
        abstract public string toString();
    }

    abstract public class Value : InstructionTerm
    {
    }
    public class Number : Value
    {
        public Number(int n) { number = n; }
        public int number;

        public override string toString() { return number.ToString(); }
    }
    public class Identificator : Value
    {
        public Identificator(string i) { id = i; }
        public string id;

        public override string toString() { return id; }
    }
    public class Boolean : Value
    {
        public Boolean(bool b) { boolean = b; }
        public bool boolean;
        public override string toString() { return boolean.ToString(); }
    }
    public class Label : InstructionTerm
    {
        public Label(string s) { label = s; }
        public string label;

        public override string toString() { return label; }
    }
    
    public enum Operation {
        NoOp,
        Assign,
        Plus,
        Minus,
        Mult,
        Div,
        Goto,
        CondGoto,
        LabelOp
    };
    // NoOp: 
    // Assign: destination := leftOperand
    // Plus: destination := leftOperand + rightOperand
    // Minus, mult, div ...
    // goto destination
    // condGoto: if leftOperand goto destination
    public struct LinearRepresentation
    {
        private static readonly Dictionary<Operation, string> s_opToStringDic = new Dictionary<Operation, string>
        {
            { Operation.NoOp, "nop" },
            { Operation.Assign, "{0} := {1}" },
            { Operation.Plus, "{0} := {1} + {2}" },
            { Operation.Minus, "{0} := {1} - {2}" },
            { Operation.Mult, "{0} := {1} * {2}" },
            { Operation.Div, "{0} := {1} / {2}" },
            { Operation.Goto, "goto {0}" },
            { Operation.CondGoto, "if {1} goto {0}" },
            { Operation.LabelOp, "{0}:" }
        };

        public Operation operation;
        public InstructionTerm destination;
        public Value leftOperand;
        public Value rightOperand;

        public LinearRepresentation(Operation op, InstructionTerm dst = null,
            Value lOp = null, Value rOp = null)
        {
            operation = op;
            destination = dst;
            leftOperand = lOp;
            rightOperand = rOp;
        }

        public String toString()
        {
            return String.Format(s_opToStringDic[operation],
                destination == null ? "" : destination.toString(),
                leftOperand == null ? "" : leftOperand.toString(),
                rightOperand == null ? "" : rightOperand.toString());
        }
    }

    // TODO: WTF EmptyNode ??
    public class LinearCode : Visitor
    {
        private static readonly string s_constantPrefix = "$";
        private static readonly string s_labelPrefix = "%l";

        // TODO: Add rest nonimplemented BinSign's operations
        private static readonly Dictionary<BinSign, Operation> s_binSignToOpDic = new Dictionary<BinSign, Operation>
        {
            { BinSign.PLUS, Operation.Plus },
            { BinSign.MINUS, Operation.Minus },
            { BinSign.MULT, Operation.Mult },
            { BinSign.DIV, Operation.Div }
        };

        private int valueCounter = 0;
        private int labelCounter = 0;

        private Value idOrNum; // LinearCode saves result in this value

        public List<LinearRepresentation> code = new List<LinearRepresentation>();
        public List<LinearRepresentation> evaluatedExpression = new List<LinearRepresentation>();

        private static Operation binSignToOp(BinSign bs)
        {
            return s_binSignToOpDic[bs];
        }

        private void moveExpressionToCode()
        {
            code.AddRange(evaluatedExpression);
            evaluatedExpression.Clear();
        }
        // combines if and loop statements
        private void branchCondition(ExprNode condition, StatementNode trueBranch, StatementNode falseBranch,
            Label beginCycle = null)
        {
            condition.Accept(this);
            Label trueCond = new Label(s_labelPrefix + labelCounter++);
            Label endCond = new Label(s_labelPrefix + labelCounter++);

            LinearRepresentation gotoCond = new LinearRepresentation(Operation.CondGoto, trueCond, idOrNum);
            evaluatedExpression.Add(gotoCond);
            if (falseBranch != null)
            {
                falseBranch.Accept(this);
            }
            evaluatedExpression.Add(new LinearRepresentation(Operation.Goto, endCond));
            evaluatedExpression.Add(new LinearRepresentation(Operation.LabelOp, trueCond));
            moveExpressionToCode();

            trueBranch.Accept(this);
            if (beginCycle != null)
            {
                evaluatedExpression.Add(new LinearRepresentation(Operation.Goto, beginCycle));
            }
            evaluatedExpression.Add(new LinearRepresentation(Operation.LabelOp, endCond));
            moveExpressionToCode();
        }

        public void Visit(IdNode id)
        {
            idOrNum = new Identificator(id.Name);
        }
        public void Visit(IntNumNode num)
        {
            idOrNum = new Number(num.Num);
        }
        public void Visit(BoolNode bNode)
        {
            idOrNum = new Boolean(bNode.Bool);
        }
        public void Visit(BinExprNode binop)
        {
            LinearRepresentation result = new LinearRepresentation(binSignToOp(binop.BinSign));
            binop.ExprLeft.Accept(this);
            result.leftOperand = idOrNum;
            binop.ExprRight.Accept(this);
            result.rightOperand = idOrNum;
            var identificator = new Identificator(s_constantPrefix + valueCounter++.ToString());
            idOrNum = identificator;
            result.destination = identificator;
            evaluatedExpression.Add(result);
        }
        public void Visit(AssignNode assNode)
        {
            if (assNode.AssOp != AssignType.Assign)
            {
                throw new NotImplementedException();
            }
            LinearRepresentation resultantAssign;
            assNode.Expr.Accept(this);
            if (evaluatedExpression.Any())
            {
                // remove last statement
                resultantAssign = evaluatedExpression.Last();
                evaluatedExpression.RemoveAt(evaluatedExpression.Count - 1);
                --valueCounter;
            }
            else
            {
                resultantAssign = new LinearRepresentation(Operation.Assign, null, idOrNum);
            }

            assNode.Id.Accept(this);
            resultantAssign.destination = idOrNum;
            evaluatedExpression.Add(resultantAssign);

            moveExpressionToCode();
        }
        public void Visit(CycleNode cycNode)
        {

        }
        public void Visit(BlockNode blNode)
        {
            for (var i = 0; i < blNode.StList.Count; ++i)
            {
                blNode.StList[i].Accept(this);
            }
        }
        public void Visit(IfNode ifNode)
        {
            branchCondition(ifNode.Condition, ifNode.TrueBranch, ifNode.ElseBranch);
        }
        public void Visit(ForNode forNode)
        {

        }
        public void Visit(RepUntNode ruNode)
        {

        }
        public void Visit(WhileNode whNode)
        {
            Label beginLabel = new Label(s_labelPrefix + labelCounter++);
            code.Add(new LinearRepresentation(Operation.LabelOp, beginLabel));
            branchCondition(whNode.Condition, whNode.Stat, null, beginLabel);
        }

        public String toString()
        {
            String text = "";
            foreach (LinearRepresentation lr in code)
            {
                text += lr.toString() + Environment.NewLine;
            }
            return text;
        }
    }
}