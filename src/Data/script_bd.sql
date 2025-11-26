CREATE SCHEMA IF NOT EXISTS cursinho_each;

SET SEARCH_PATH = cursinho_each;

CREATE TABLE IF NOT EXISTS turma(
	ano INTEGER NOT NULL,
	periodo CHAR(1) NOT NULL CHECK(periodo in ('M', 'V', 'N')),
	capacidade INTEGER,
	PRIMARY KEY (ano, periodo)
);

CREATE TABLE IF NOT EXISTS pessoa(
	cpf CHAR(11) NOT NULL,
	email VARCHAR(100) NOT NULL,
	telefone VARCHAR(11),
	nome VARCHAR(100) NOT NULL,
	endereco VARCHAR(255),
	data_nascimento DATE,
	PRIMARY KEY (cpf)
);

CREATE TABLE IF NOT EXISTS aluno(
	cpf CHAR(11) NOT NULL,
	ano_escolar INTEGER,
	matriculado BOOLEAN DEFAULT TRUE,
	desligado_motivo VARCHAR(100),
	representante_legal  CHAR(11) NOT NULL,
	turma_ano INTEGER NOT NULL,
	turma_periodo CHAR(1) NOT NULL, ----------------------------------------- UM ALUNO SÓ PODE PARTICIPAR UM ANO?
	PRIMARY KEY (cpf),
	FOREIGN KEY (cpf) REFERENCES pessoa(cpf),
	FOREIGN KEY (representante_legal) REFERENCES pessoa(cpf),
	FOREIGN KEY (turma_ano, turma_periodo) REFERENCES turma(ano, periodo)
);

CREATE TABLE IF NOT EXISTS professor(
	cpf CHAR(11) NOT NULL,
	tipo CHAR(1) NOT NULL CHECK(tipo IN ('R', 'M', 'P')),
	PRIMARY KEY (cpf),
	FOREIGN KEY (cpf) REFERENCES pessoa(cpf)
);

CREATE TABLE IF NOT EXISTS materia(
	nome VARCHAR(50) NOT NULL,
	area VARCHAR(50),
	PRIMARY KEY (nome)
);

CREATE TABLE IF NOT EXISTS prova(
	id BIGSERIAL NOT NULL,
	nome VARCHAR(100) NOT NULL,
	fase INTEGER,
	tipo CHAR(1) CHECK(tipo in ('ENEM', 'FUVEST', 'UNICAMP', 'OUTRAS')),
	PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS evento(
	id BIGSERIAL NOT NULL,
	data DATE,
	hora_inicio TIME,
	duracao_minutos INTEGER,
	tipo CHAR(1) CHECK(tipo in ('A', 'S', 'O')),
	prova_id BIGINT,
	PRIMARY KEY (id),
	FOREIGN KEY (prova_id) REFERENCES prova(id)
);

CREATE TABLE IF NOT EXISTS 	evento_materia (
	evento_id BIGINT NOT NULL,
	materia_nome VARCHAR(50) NOT NULL,
	PRIMARY KEY (evento_id, materia_nome),
	FOREIGN KEY (evento_id) REFERENCES evento(id),
	FOREIGN KEY (materia_nome) REFERENCES materia(nome)
);

CREATE TABLE IF NOT EXISTS questao(
	prova_id BIGINT NOT NULL,
	numero INTEGER NOT NULL,
	enunciado VARCHAR(500) NOT NULL,
	gabarito VARCHAR(500) NOT NULL,
	PRIMARY KEY (prova_id, numero),
	FOREIGN KEY (prova_id) REFERENCES prova(id)
);

CREATE TABLE IF NOT EXISTS professor_materia_turma (
	professor_cpf CHAR(11) NOT NULL,
	materia_nome VARCHAR(50) NOT NULL,
	turma_ano INTEGER NOT NULL,
	turma_periodo CHAR(1) NOT NULL,
	PRIMARY KEY (professor_cpf, materia_nome, turma_ano, turma_periodo),
	FOREIGN KEY (professor_cpf) REFERENCES professor(cpf),
	FOREIGN KEY (materia_nome) REFERENCES materia(nome),
	FOREIGN KEY (turma_ano, turma_periodo) REFERENCES turma(ano, periodo)
);

CREATE TABLE IF NOT EXISTS aluno_evento (
	aluno_cpf CHAR(11) NOT NULL,
	evento_id BIGINT NOT NULL,
	presente BOOLEAN DEFAULT FALSE,
	PRIMARY KEY (aluno_cpf, evento_id),
	FOREIGN KEY (aluno_cpf) REFERENCES aluno(cpf),
	FOREIGN KEY (evento_id) REFERENCES evento(id)
);

CREATE TABLE IF NOT EXISTS aluno_questao (
	aluno_cpf CHAR(11) NOT NULL,
	questao_prova_id BIGINT NOT NULL,
	questao_numero INTEGER NOT NULL,
	tempo_resposta_seg INTEGER NOT NULL,
	alternativa CHAR(1) NOT NULL CHECK(alternativa IN ('A', 'B', 'C', 'D', 'E')),
	PRIMARY KEY (aluno_cpf, questao_prova_id, questao_numero),
	FOREIGN KEY (aluno_cpf) REFERENCES aluno(cpf),
	FOREIGN KEY (questao_prova_id, questao_numero) REFERENCES questao(prova_id, numero)
);

CREATE TABLE IF NOT EXISTS questao_materia (
	questao_prova_id BIGINT NOT NULL,
	questao_numero INTEGER NOT NULL,
	materia_nome VARCHAR(50) NOT NULL,
	subarea VARCHAR(50),
	PRIMARY KEY (questao_prova_id, questao_numero, materia_nome),
	FOREIGN KEY (questao_prova_id, questao_numero) REFERENCES questao(prova_id, numero),
	FOREIGN KEY (materia_nome) REFERENCES materia(nome)
);

CREATE TABLE IF NOT EXISTS evento_turma (
    evento_id BIGINT NOT NULL,
    turma_ano INTEGER NOT NULL,
    turma_periodo CHAR(1) NOT NULL,
    PRIMARY KEY (evento_id, turma_ano, turma_periodo),
	FOREIGN KEY (turma_ano, turma_periodo) REFERENCES turma(ano, periodo),
	FOREIGN KEY (evento_id) REFERENCES evento(id)
);