%using SimpleLang;
%{
// Ёти объ€влени€ добавл€ютс€ в класс GPPGParser, представл€ющий собой парсер, генерируемый системой gppg
	public BlockNode root;
    public Parser(AbstractScanner<SimpleParser.ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union {
		public double dVal;
		public int iVal;
		public string sVal;
		public Node nVal;
		public ExprNode eVal;
		public StatementNode stVal;
		public BlockNode blVal;
		public BinSign bsVal;
}

//%using ProgramTree

%namespace SimpleParser

%token BEGIN END CYCLE INUM RNUM ID ASSIGN SEMICOLON LBRACE RBRACE PLUS MINUS MULT DIV VAR COMMA IF THEN ELSE WRITE FOR TO DO REPEAT UNTIL WHILE
%token LS GT LE GE EQ NE
%token <iVal> INUM
%token <dVal> RNUM
%token <sVal> ID

%type <eVal> expr ident bin_expr
%type <stVal> assign statement cycle if_st rep_unt while_st for_st
%type <blVal> stlist block
%type <bsVal> bin_sign

%nonassoc IFX
%nonassoc ELSE

%%


progr   : block { root = $1; }
		;

stlist	: statement
		{
			$$ = new BlockNode($1);
		}
		| stlist statement
		{
			$1.Add($2);
			$$ = $1;
		}
		;

statement: assign SEMICOLON
		| block
		| cycle
		| if_st
		| for_st
		| rep_unt
		| while_st
		;

ident 	: ID
		;
	
assign 	: ident ASSIGN expr
		{
			$$ = new AssignNode($1 as IdNode, $3);
		}
		;

block	: BEGIN stlist END
		{
			$$ = $2;
		}
		;

cycle	: CYCLE expr statement
		{
			$$ = new CycleNode($2, $3);
		}
		;

if_st   : IF expr THEN statement %prec IFX
		{
			$$ = new IfNode($2, $4);
		}
		| IF expr THEN statement ELSE statement
		{
			$$ = new IfNode($2, $4, $6);
		}
		;

rep_unt : REPEAT stlist UNTIL bin_expr
		{
			$$ = new RepUntNode($2, $4 as BinExprNode);
		}
		;

while_st: WHILE bin_expr DO statement
		{
			$$ = new WhileNode($2 as BinExprNode, $4);
		}
		;

for_st	: FOR assign TO expr DO statement
		{
			$$ = new ForNode($2 as AssignNode, $4, $6);
		}
		;

bin_sign: LS
		| GT
		| LE
		| GE
		| EQ
		| NE
		;

bin_expr: expr bin_sign expr
		{
			$$ = new BinExprNode( $1, $2, $3);
		}
		;

expr	: m_d
		| expr PLUS m_d
		| expr MINUS m_d
		;

m_d		: in_br
		| m_d MULT in_br
		| m_d DIV in_br
		;

in_br	: ident
		| INUM
		| LBRACE expr RBRACE
		;
%%