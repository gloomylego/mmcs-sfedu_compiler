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
}

//%using ProgramTree

%namespace SimpleParser

%token BEGIN END CYCLE INUM RNUM ID ASSIGN SEMICOLON LBRACE RBRACE PLUS MINUS MULT DIV VAR COMMA IF THEN ELSE WRITE FOR TO DO REPEAT UNTIL WHILE
%token LS GT LE GE EQ NE
%token <iVal> INUM
%token <dVal> RNUM
%token <sVal> ID

%type <eVal> expr ident
%type <stVal> assign statement cycle
%type <blVal> stlist block

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
		| var_id SEMICOLON
		| if_st
		| write_st SEMICOLON
		| for_st
		| rep_unt
		| while_st
		;

ident 	: ID
		;
	
assign 	: ident ASSIGN expr
		;

block	: BEGIN stlist END
		;

cycle	: CYCLE expr statement
		;

if_st   : IF expr THEN statement %prec IFX
		| IF expr THEN statement else_st
		;
		
else_st : ELSE statement
		;

rep_unt : REPEAT stlist UNTIL bin_expr
		;

while_st: WHILE bin_expr DO statement
		;

write_st: WRITE LBRACE expr RBRACE
		;

for_st	: FOR assign TO expr DO statement
		;

bin_sign: LS
		| GT
		| LE
		| GE
		| EQ
		| NE
		;

bin_expr: expr
		| bin_expr bin_sign expr
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

enum    : ident
		| assign
		| enum COMMA ident
		| enum COMMA assign
		;

var_id	: VAR enum
		;

%%
