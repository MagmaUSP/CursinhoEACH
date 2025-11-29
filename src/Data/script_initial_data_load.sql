SET SEARCH_PATH = cursinho_each;

-- =========================
-- TABELA: turma (30)
-- =========================
INSERT INTO turma (ano, periodo, capacidade) VALUES
  (2024, 'V', 5),
  (2024, 'N', 5),
  (2025, 'V', 5),
  (2025, 'N', 5),
  (2026, 'V', 5),
  (2026, 'N', 5);

-- =========================
-- TABELA: pessoa (30)
-- =========================
INSERT INTO pessoa (cpf, email, telefone, nome, endereco, data_nascimento) VALUES
  ('00000000001', 'ana.silva@gmail.com', '11987654321', 'Ana Paula Silva', 'Rua das Flores, 123 - Itaquera, São Paulo - SP', '1981-02-15'),
  ('00000000002', 'carlos.souza@hotmail.com', '11976543210', 'Carlos Eduardo Souza', 'Av. Paulista, 1500 - Bela Vista, São Paulo - SP', '1982-03-22'),
  ('00000000003', 'maria.santos@outlook.com', '11965432109', 'Maria Fernanda Santos', 'Rua Augusta, 250 - Consolação, São Paulo - SP', '1983-04-10'),
  ('00000000004', 'joao.oliveira@yahoo.com', '11954321098', 'João Pedro Oliveira', 'Av. Aricanduva, 5555 - Aricanduva, São Paulo - SP', '1984-05-08'),
  ('00000000005', 'juliana.costa@gmail.com', '11943210987', 'Juliana Rodrigues Costa', 'Rua Vergueiro, 3185 - Vila Mariana, São Paulo - SP', '1985-06-17'),
  ('00000000006', 'pedro.lima@hotmail.com', '11932109876', 'Pedro Henrique Lima', 'Av. Celso Garcia, 1200 - Tatuapé, São Paulo - SP', '1986-07-25'),
  ('00000000007', 'beatriz.alves@outlook.com', '11921098765', 'Beatriz Almeida Alves', 'Rua da Mooca, 1950 - Mooca, São Paulo - SP', '1987-08-13'),
  ('00000000008', 'rafael.pereira@gmail.com', '11910987654', 'Rafael Santos Pereira', 'Av. Ibirapuera, 3103 - Indianópolis, São Paulo - SP', '1988-09-30'),
  ('00000000009', 'camila.ferreira@yahoo.com', '11909876543', 'Camila Oliveira Ferreira', 'Rua Oscar Freire, 379 - Jardins, São Paulo - SP', '1989-10-19'),
  ('00000000010', 'lucas.martins@gmail.com', '11998765432', 'Lucas Gabriel Martins', 'Av. Brigadeiro Faria Lima, 1811 - Jardim Paulistano, São Paulo - SP', '1990-11-27'),
  ('00000000011', 'fernanda.rocha@hotmail.com', '11987654322', 'Fernanda Costa Rocha', 'Rua Haddock Lobo, 595 - Cerqueira César, São Paulo - SP', '1991-12-05'),
  ('00000000012', 'bruno.carvalho@outlook.com', '11976543211', 'Bruno Henrique Carvalho', 'Av. Rebouças, 3970 - Pinheiros, São Paulo - SP', '1992-01-14'),
  ('00000000013', 'larissa.gomes@gmail.com', '11965432110', 'Larissa Mendes Gomes', 'Rua Frei Caneca, 569 - Consolação, São Paulo - SP', '1993-02-23'),
  ('00000000014', 'thiago.ribeiro@yahoo.com', '11954321099', 'Thiago Alves Ribeiro', 'Av. São João, 473 - Centro, São Paulo - SP', '1994-03-18'),
  ('00000000015', 'gabriela.dias@gmail.com', '11943210988', 'Gabriela Souza Dias', 'Rua 25 de Março, 911 - Centro, São Paulo - SP', '1995-04-29'),
  ('00000000016', 'rodrigo.araujo@hotmail.com', '11932109877', 'Rodrigo Fernandes Araújo', 'Av. Sapopemba, 9064 - Sapopemba, São Paulo - SP', '1996-05-11'),
  ('00000000017', 'patricia.campos@outlook.com', '11921098766', 'Patricia Lima Campos', 'Rua Dr. Cesário Mota Jr, 112 - Vila Buarque, São Paulo - SP', '1997-06-07'),
  ('00000000018', 'diego.nascimento@gmail.com', '11910987655', 'Diego Santos Nascimento', 'Av. Tiradentes, 615 - Luz, São Paulo - SP', '1998-07-16'),
  ('00000000019', 'amanda.cardoso@yahoo.com', '11909876544', 'Amanda Cristina Cardoso', 'Rua Maria Paula, 123 - Bixiga, São Paulo - SP', '1999-08-24'),
  ('00000000020', 'marcelo.freitas@gmail.com', '11998765433', 'Marcelo Luiz Freitas', 'Av. Ipiranga, 344 - República, São Paulo - SP', '2000-09-02'),
  ('00000000021', 'aline.barros@hotmail.com', '11987654323', 'Aline Rodrigues Barros', 'Rua Teodoro Sampaio, 2050 - Pinheiros, São Paulo - SP', '1981-10-12'),
  ('00000000022', 'gustavo.melo@outlook.com', '11976543212', 'Gustavo Henrique Melo', 'Av. Rudge, 315 - Bom Retiro, São Paulo - SP', '1982-11-20'),
  ('00000000023', 'renata.monteiro@gmail.com', '11965432111', 'Renata Alves Monteiro', 'Rua Líbero Badaró, 425 - Centro, São Paulo - SP', '1983-12-28'),
  ('00000000024', 'felipe.teixeira@yahoo.com', '11954321100', 'Felipe Costa Teixeira', 'Av. Rio Branco, 1269 - Campos Elíseos, São Paulo - SP', '1984-01-06'),
  ('00000000025', 'tatiana.pinto@gmail.com', '11943210989', 'Tatiana Fernandes Pinto', 'Rua Voluntários da Pátria, 1348 - Santana, São Paulo - SP', '1985-02-14'),
  ('00000000026', 'andre.moreira@hotmail.com', '11932109878', 'André Luis Moreira', 'Av. Cruzeiro do Sul, 1100 - Canindé, São Paulo - SP', '1986-03-25'),
  ('00000000027', 'vanessa.cavalcante@outlook.com', '11921098767', 'Vanessa Silva Cavalcante', 'Rua do Gasômetro, 93 - Brás, São Paulo - SP', '1987-04-03'),
  ('00000000028', 'eduardo.nunes@gmail.com', '11910987656', 'Eduardo Augusto Nunes', 'Av. Rangel Pestana, 2991 - Brás, São Paulo - SP', '1988-05-11'),
  ('00000000029', 'priscila.barbosa@yahoo.com', '11909876545', 'Priscila Santos Barbosa', 'Rua Florêncio de Abreu, 697 - Centro, São Paulo - SP', '1989-06-19'),
  ('00000000030', 'vinicius.santana@gmail.com', '11998765434', 'Vinícius Almeida Santana', 'Av. São Luís, 187 - República, São Paulo - SP', '1990-07-27');

-- =========================
-- TABELA: aluno (20)
-- =========================
INSERT INTO aluno (cpf, ano_escolar, matriculado, desligado_motivo, representante_legal, turma_ano, turma_periodo) VALUES
  ('00000000011', 3, TRUE, NULL, '00000000003', 2024, 'V'),
  ('00000000012', 1, FALSE, NULL, '00000000001', 2024, 'N'),
  ('00000000013', 2, FALSE, 'Desistencia', '00000000004', 2024, 'V'),
  ('00000000014', 3, TRUE, NULL, '00000000003', 2024, 'V'),
  ('00000000015', 1, TRUE, NULL, '00000000003', 2024, 'N'),
  ('00000000016', 2, TRUE, NULL, '00000000001', 2025, 'N'),
  ('00000000017', 3, TRUE, NULL, '00000000001', 2025, 'V'),
  ('00000000018', 1, FALSE, 'Aprovado em segunda chamada', '00000000007', 2025, 'N'),
  ('00000000019', 2, TRUE, NULL, '00000000005', 2025, 'N'),
  ('00000000020', 3, TRUE, NULL, '00000000004', 2025, 'V'),
  ('00000000021', 1, TRUE, NULL, '00000000002', 2025, 'N'),
  ('00000000022', 2, TRUE, NULL, '00000000003', 2025, 'V'),
  ('00000000023', 3, TRUE, NULL, '00000000001', 2025, 'V'),
  ('00000000024', 1, FALSE, 'Desistencia', '00000000002', 2026, 'N'),
  ('00000000025', 2, TRUE, NULL, '00000000001', 2026, 'V'),
  ('00000000026', 3, FALSE, 'Codigo de conduta', '00000000001', 2026, 'V'),
  ('00000000027', 1, TRUE, NULL, '00000000004', 2026, 'N'),
  ('00000000028', 2, TRUE, NULL, '00000000003', 2026, 'N'),
  ('00000000029', 3, TRUE, NULL, '00000000001', 2026, 'N'),
  ('00000000030', 1, FALSE, 'Desistencia', '00000000001', 2026, 'N');

-- =========================
-- TABELA: professor (7)
-- =========================
INSERT INTO professor (cpf, tipo) VALUES
  ('00000000006', 'R'),
  ('00000000007', 'R'),
  ('00000000008', 'R'),
  ('00000000009', 'M'),
  ('00000000010', 'P'),
  ('00000000011', 'R'),
  ('00000000012', 'R');

-- =========================
-- TABELA: materia (30)
-- =========================
INSERT INTO materia (nome, area) VALUES
  ('Álgebra', 'E'),
  ('Geometria', 'E'),
  ('História do Brasil', 'H'),
  ('História Geral', 'H'),
  ('Geografia do Brasil', 'H'),
  ('Geografia Geral', 'H'),
  ('Biologia', 'N'),
  ('Química Orgânica', 'N'),
  ('Química Inorgânica', 'N'),
  ('Física', 'N'),
  ('Inglês', 'L'),
  ('Atualidades', 'O'),
  ('Português', 'L'),
  ('Redação', 'L'),
  ('Literatura', 'L');

-- =========================
-- TABELA: prova (15 provas realistas)
-- =========================
INSERT INTO prova (id, nome, fase, tipo) VALUES
  -- 2024 - ENEM
  (1, 'ENEM 2024 - Dia 1', 1, 'E'),
  (2, 'ENEM 2024 - Dia 2', 1, 'E'),
  
  -- 2024 - FUVEST
  (3, 'FUVEST 2024 - 1ª Fase', 1, 'F'),
  (4, 'FUVEST 2024 - 2ª Fase (Dia 1)', 2, 'F'),
  (5, 'FUVEST 2024 - 2ª Fase (Dia 2)', 2, 'F'),
  
  -- 2024 - UNICAMP
  (6, 'UNICAMP 2024 - 1ª Fase', 1, 'U'),
  (7, 'UNICAMP 2024 - 2ª Fase (Dia 1)', 2, 'U'),
  (8, 'UNICAMP 2024 - 2ª Fase (Dia 2)', 2, 'U'),
  
  -- 2025 - ENEM
  (9, 'ENEM 2025 - Dia 1', 1, 'E'),
  (10, 'ENEM 2025 - Dia 2', 1, 'E'),
  
  -- 2025 - FUVEST
  (11, 'FUVEST 2025 - 1ª Fase', 1, 'F'),
  (12, 'FUVEST 2025 - 2ª Fase (Dia 1)', 2, 'F'),
  (13, 'FUVEST 2025 - 2ª Fase (Dia 2)', 2, 'F'),
  
  -- 2025 - UNICAMP
  (14, 'UNICAMP 2025 - 1ª Fase', 1, 'U'),
  (15, 'UNICAMP 2025 - 2ª Fase', 2, 'U');

-- =========================
-- TABELA: evento
-- =========================
INSERT INTO evento (id, data, hora_inicio, duracao_minutos, tipo, prova_id) VALUES
  -- SIMULADOS 2024 (IDs 1-8)
  (1, '2024-03-15', '14:00:00', 270, 'S', 1),  -- ENEM Dia 1
  (2, '2024-03-16', '14:00:00', 270, 'S', 2),  -- ENEM Dia 2
  (3, '2024-06-20', '13:00:00', 300, 'S', 3),  -- FUVEST 1ª Fase
  (4, '2024-09-10', '13:00:00', 240, 'S', 4),  -- FUVEST 2ª Fase Dia 1
  (5, '2024-09-11', '13:00:00', 240, 'S', 5),  -- FUVEST 2ª Fase Dia 2
  (6, '2024-04-12', '14:00:00', 300, 'S', 6),  -- UNICAMP 1ª Fase
  (7, '2024-07-15', '13:00:00', 240, 'S', 7),  -- UNICAMP 2ª Fase Dia 1
  (8, '2024-07-16', '13:00:00', 240, 'S', 8),  -- UNICAMP 2ª Fase Dia 2
  
  -- SIMULADOS 2025 (IDs 9-15)
  (9, '2025-10-10', '14:00:00', 270, 'S', 9),   -- ENEM Dia 1
  (10, '2025-10-11', '14:00:00', 270, 'S', 10), -- ENEM Dia 2
  (11, '2025-05-20', '13:00:00', 300, 'S', 11), -- FUVEST 1ª Fase
  (12, '2025-08-12', '13:00:00', 240, 'S', 12), -- FUVEST 2ª Fase Dia 1
  (13, '2025-08-13', '13:00:00', 240, 'S', 13), -- FUVEST 2ª Fase Dia 2
  (14, '2025-09-25', '14:00:00', 300, 'S', 14), -- UNICAMP 1ª Fase
  (15, '2025-11-10', '13:00:00', 300, 'S', 15), -- UNICAMP 2ª Fase
  
  -- AULAS 2024 VESPERTINO (IDs 16-25)
  (16, '2024-03-04', '14:00:00', 180, 'A', NULL),
  (17, '2024-03-05', '14:00:00', 180, 'A', NULL),
  (18, '2024-03-06', '14:00:00', 180, 'A', NULL),
  (19, '2024-03-07', '14:00:00', 180, 'A', NULL),
  (20, '2024-03-08', '14:00:00', 180, 'A', NULL),
  (21, '2024-03-11', '14:00:00', 180, 'A', NULL),
  (22, '2024-03-12', '14:00:00', 180, 'A', NULL),
  (23, '2024-03-13', '14:00:00', 180, 'A', NULL),
  (24, '2024-03-14', '14:00:00', 180, 'A', NULL),
  (25, '2024-03-15', '14:00:00', 180, 'A', NULL),
  
  -- AULAS 2024 NOTURNO (IDs 26-35)
  (26, '2024-03-04', '19:00:00', 180, 'A', NULL),
  (27, '2024-03-05', '19:00:00', 180, 'A', NULL),
  (28, '2024-03-06', '19:00:00', 180, 'A', NULL),
  (29, '2024-03-07', '19:00:00', 180, 'A', NULL),
  (30, '2024-03-08', '19:00:00', 180, 'A', NULL),
  (31, '2024-03-11', '19:00:00', 180, 'A', NULL),
  (32, '2024-03-12', '19:00:00', 180, 'A', NULL),
  (33, '2024-03-13', '19:00:00', 180, 'A', NULL),
  (34, '2024-03-14', '19:00:00', 180, 'A', NULL),
  (35, '2024-03-15', '19:00:00', 180, 'A', NULL),
  
  -- AULAS 2025 VESPERTINO (IDs 36-45)
  (36, '2025-03-03', '14:00:00', 180, 'A', NULL),
  (37, '2025-03-04', '14:00:00', 180, 'A', NULL),
  (38, '2025-03-05', '14:00:00', 180, 'A', NULL),
  (39, '2025-03-06', '14:00:00', 180, 'A', NULL),
  (40, '2025-03-07', '14:00:00', 180, 'A', NULL),
  (41, '2025-03-10', '14:00:00', 180, 'A', NULL),
  (42, '2025-03-11', '14:00:00', 180, 'A', NULL),
  (43, '2025-03-12', '14:00:00', 180, 'A', NULL),
  (44, '2025-03-13', '14:00:00', 180, 'A', NULL),
  (45, '2025-03-14', '14:00:00', 180, 'A', NULL),
  
  -- AULAS 2025 NOTURNO (IDs 46-55)
  (46, '2025-03-03', '19:00:00', 180, 'A', NULL),
  (47, '2025-03-04', '19:00:00', 180, 'A', NULL),
  (48, '2025-03-05', '19:00:00', 180, 'A', NULL),
  (49, '2025-03-06', '19:00:00', 180, 'A', NULL),
  (50, '2025-03-07', '19:00:00', 180, 'A', NULL),
  (51, '2025-03-10', '19:00:00', 180, 'A', NULL),
  (52, '2025-03-11', '19:00:00', 180, 'A', NULL),
  (53, '2025-03-12', '19:00:00', 180, 'A', NULL),
  (54, '2025-03-13', '19:00:00', 180, 'A', NULL),
  (55, '2025-03-14', '19:00:00', 180, 'A', NULL);

-- =========================
-- TABELA: evento_materia
-- =========================
INSERT INTO evento_materia (evento_id, materia_nome) VALUES
 -- 2024 VESPERTINO - Semana 1 (eventos 16-20)
  (16, 'Português'), (16, 'Álgebra'), (16, 'História do Brasil'),
  (17, 'Redação'), (17, 'Álgebra'), (17, 'Geografia do Brasil'),
  (18, 'Literatura'), (18, 'Geometria'), (18, 'Química Orgânica'),
  (19, 'Inglês'), (19, 'Física'), (19, 'Biologia'),
  (20, 'Atualidades'), (20, 'Química Inorgânica'), (20, 'História Geral'),
  
  -- 2024 VESPERTINO - Semana 2 (eventos 21-25)
  (21, 'Português'), (21, 'Álgebra'), (21, 'Geografia Geral'),
  (22, 'Redação'), (22, 'Geometria'), (22, 'História do Brasil'),
  (23, 'Literatura'), (23, 'Física'), (23, 'Química Orgânica'),
  (24, 'Inglês'), (24, 'Álgebra'), (24, 'Biologia'),
  (25, 'Atualidades'), (25, 'Química Inorgânica'), (25, 'Geografia do Brasil'),
  
  -- 2024 NOTURNO - Semana 1 (eventos 26-30)
  (26, 'Álgebra'), (26, 'Português'), (26, 'Física'),
  (27, 'Álgebra'), (27, 'Literatura'), (27, 'Química Orgânica'),
  (28, 'Geometria'), (28, 'Redação'), (28, 'Biologia'),
  (29, 'Física'), (29, 'Inglês'), (29, 'História do Brasil'),
  (30, 'Química Inorgânica'), (30, 'Atualidades'), (30, 'Geografia do Brasil'),
  
  -- 2024 NOTURNO - Semana 2 (eventos 31-35)
  (31, 'Álgebra'), (31, 'Português'), (31, 'História Geral'),
  (32, 'Álgebra'), (32, 'Literatura'), (32, 'Geografia Geral'),
  (33, 'Geometria'), (33, 'Redação'), (33, 'Química Orgânica'),
  (34, 'Física'), (34, 'Inglês'), (34, 'Biologia'),
  (35, 'Química Inorgânica'), (35, 'Atualidades'), (35, 'História do Brasil'),
  
  -- 2025 VESPERTINO - Semana 1 (eventos 36-40)
  (36, 'Português'), (36, 'Álgebra'), (36, 'História do Brasil'),
  (37, 'Redação'), (37, 'Álgebra'), (37, 'Geografia do Brasil'),
  (38, 'Literatura'), (38, 'Geometria'), (38, 'Química Orgânica'),
  (39, 'Inglês'), (39, 'Física'), (39, 'Biologia'),
  (40, 'Atualidades'), (40, 'Química Inorgânica'), (40, 'História Geral'),
  
  -- 2025 VESPERTINO - Semana 2 (eventos 41-45)
  (41, 'Português'), (41, 'Álgebra'), (41, 'Geografia Geral'),
  (42, 'Redação'), (42, 'Geometria'), (42, 'História do Brasil'),
  (43, 'Literatura'), (43, 'Física'), (43, 'Química Orgânica'),
  (44, 'Inglês'), (44, 'Álgebra'), (44, 'Biologia'),
  (45, 'Atualidades'), (45, 'Química Inorgânica'), (45, 'Geografia do Brasil'),
  
  -- 2025 NOTURNO - Semana 1 (eventos 46-50)
  (46, 'Álgebra'), (46, 'Português'), (46, 'Física'),
  (47, 'Álgebra'), (47, 'Literatura'), (47, 'Química Orgânica'),
  (48, 'Geometria'), (48, 'Redação'), (48, 'Biologia'),
  (49, 'Física'), (49, 'Inglês'), (49, 'História do Brasil'),
  (50, 'Química Inorgânica'), (50, 'Atualidades'), (50, 'Geografia do Brasil'),
  
  -- 2025 NOTURNO - Semana 2 (eventos 51-55)
  (51, 'Álgebra'), (51, 'Português'), (51, 'História Geral'),
  (52, 'Álgebra'), (52, 'Literatura'), (52, 'Geografia Geral'),
  (53, 'Geometria'), (53, 'Redação'), (53, 'Química Orgânica'),
  (54, 'Física'), (54, 'Inglês'), (54, 'Biologia'),
  (55, 'Química Inorgânica'), (55, 'Atualidades'), (55, 'História do Brasil');

-- =========================
-- TABELA: questao (45 questões - 3 por prova)
-- =========================
INSERT INTO questao (prova_id, numero, enunciado, gabarito) VALUES
  -- ENEM 2024 - Dia 1 (Linguagens e Ciências Humanas)
  (1, 1, 'Texto sobre interpretação de gêneros textuais e suas características no contexto comunicativo contemporâneo.', 'A'),
  (1, 2, 'Análise de fatos históricos relacionados à formação territorial brasileira e seus impactos socioculturais.', 'C'),
  (1, 3, 'Questão sobre geografia urbana e processos de urbanização nas metrópoles brasileiras.', 'D'),
  
  -- ENEM 2024 - Dia 2 (Ciências da Natureza e Matemática)
  (2, 1, 'Problema envolvendo equações do segundo grau aplicadas a situações cotidianas.', 'B'),
  (2, 2, 'Questão sobre química orgânica: reações de esterificação e suas aplicações industriais.', 'E'),
  (2, 3, 'Análise de gráficos sobre movimentos retilíneos uniformemente variados.', 'A'),
  
  -- FUVEST 2024 - 1ª Fase
  (3, 1, 'Interpretação de texto literário de Machado de Assis com foco em elementos narrativos.', 'C'),
  (3, 2, 'Questão sobre a Revolução Francesa e suas influências nos movimentos de independência.', 'B'),
  (3, 3, 'Cálculo de probabilidade em experimentos com distribuição binomial.', 'D'),
  
  -- FUVEST 2024 - 2ª Fase Dia 1 (Português e Redação) - AGORA OBJETIVAS
  (4, 1, 'Análise comparativa entre dois textos sobre sustentabilidade ambiental.', 'B'),
  (4, 2, 'Questão gramatical sobre regência verbal e nominal em contextos formais de comunicação.', 'D'),
  (4, 3, 'Interpretação de texto argumentativo sobre o papel da tecnologia na educação contemporânea.', 'A'),
  
  -- FUVEST 2024 - 2ª Fase Dia 2 (Exatas) - AGORA OBJETIVAS
  (5, 1, 'Problema complexo de geometria analítica envolvendo cônicas e suas propriedades.', 'C'),
  (5, 2, 'Questão de física sobre termodinâmica e transformações gasosas.', 'E'),
  (5, 3, 'Química: equilíbrio químico e cálculo de pH em soluções tampão.', 'B'),
  
  -- UNICAMP 2024 - 1ª Fase
  (6, 1, 'Texto sobre movimentos sociais no Brasil contemporâneo e cidadania.', 'B'),
  (6, 2, 'Análise de poema modernista com foco em recursos estilísticos.', 'D'),
  (6, 3, 'Questão sobre ecologia e impactos ambientais da ação humana.', 'A'),
  
  -- UNICAMP 2024 - 2ª Fase Dia 1 - AGORA OBJETIVAS
  (7, 1, 'Questão sobre literatura brasileira: análise de romance regionalista.', 'A'),
  (7, 2, 'História: processo de redemocratização no Brasil pós-ditadura militar.', 'C'),
  (7, 3, 'Geografia: questões agrárias e reforma agrária no Brasil.', 'E'),
  
  -- UNICAMP 2024 - 2ª Fase Dia 2 - AGORA OBJETIVAS
  (8, 1, 'Matemática: sistemas lineares e matrizes aplicados a problemas reais.', 'D'),
  (8, 2, 'Física: ondas eletromagnéticas e aplicações tecnológicas.', 'B'),
  (8, 3, 'Química orgânica: polímeros e suas aplicações industriais.', 'A'),
  
  -- ENEM 2025 - Dia 1
  (9, 1, 'Interpretação de charge sobre questões políticas contemporâneas.', 'C'),
  (9, 2, 'Filosofia: conceitos de ética e moral na sociedade moderna.', 'B'),
  (9, 3, 'Sociologia: análise de movimentos culturais urbanos.', 'E'),
  
  -- ENEM 2025 - Dia 2
  (10, 1, 'Análise de gráficos estatísticos sobre dados populacionais.', 'D'),
  (10, 2, 'Biologia: genética mendeliana e hereditariedade.', 'A'),
  (10, 3, 'Química inorgânica: reações de oxirredução e pilhas.', 'C'),
  
  -- FUVEST 2025 - 1ª Fase
  (11, 1, 'Literatura: análise de crônica de Rubem Braga.', 'B'),
  (11, 2, 'História: Era Vargas e suas transformações políticas.', 'D'),
  (11, 3, 'Matemática: funções exponenciais e logarítmicas.', 'A'),
  
  -- FUVEST 2025 - 2ª Fase Dia 1 - AGORA OBJETIVAS
  (12, 1, 'Análise sintática de período composto em texto jornalístico.', 'C'),
  (12, 2, 'Literatura comparada: romantismo e realismo brasileiro.', 'E'),
  (12, 3, 'Interpretação de texto sobre desigualdade social no Brasil.', 'B'),
  
  -- FUVEST 2025 - 2ª Fase Dia 2 - AGORA OBJETIVAS
  (13, 1, 'Trigonometria: resolução de triângulos e aplicações.', 'A'),
  (13, 2, 'Física: eletromagnetismo e Lei de Faraday.', 'D'),
  (13, 3, 'Química: cinética química e fatores que afetam velocidade.', 'C'),
  
  -- UNICAMP 2025 - 1ª Fase
  (14, 1, 'Interpretação de texto sobre mudanças climáticas globais.', 'C'),
  (14, 2, 'História: colonialismo e suas consequências na América Latina.', 'E'),
  (14, 3, 'Biologia: evolução e seleção natural.', 'B'),
  
  -- UNICAMP 2025 - 2ª Fase - AGORA OBJETIVAS
  (15, 1, 'Questão interdisciplinar sobre energia renovável (Física + Química).', 'B'),
  (15, 2, 'Matemática aplicada: modelagem de fenômenos naturais.', 'D'),
  (15, 3, 'Interpretação de tema dissertativo: O futuro da educação no Brasil.', 'A');

-- =========================
-- TABELA: professor_materia_turma
-- =========================
INSERT INTO professor_materia_turma (professor_cpf, materia_nome, turma_ano, turma_periodo) VALUES
  -- Matemática e suas áreas
  ('00000000006', 'Álgebra', 2024, 'V'),
  ('00000000006', 'Geometria', 2024, 'V'),
  ('00000000006', 'Álgebra', 2025, 'N'),
  ('00000000006', 'Geometria', 2025, 'N'),
  
  -- Ciências da Natureza
  ('00000000007', 'Física', 2024, 'V'),
  ('00000000007', 'Química Orgânica', 2024, 'V'),
  ('00000000007', 'Química Inorgânica', 2024, 'V'),
  ('00000000008', 'Física', 2025, 'N'),
  ('00000000009', 'Química Orgânica', 2025, 'N'),
  ('00000000011', 'Química Inorgânica', 2025, 'N'),
  
  -- Biologia e Atualidades
  ('00000000008', 'Biologia', 2024, 'V'),
  ('00000000009', 'Atualidades', 2024, 'V'),
  ('00000000008', 'Biologia', 2025, 'N'),
  ('00000000009', 'Atualidades', 2025, 'N'),
  
  -- Linguagens
  ('00000000010', 'Português', 2024, 'V'),
  ('00000000010', 'Redação', 2024, 'V'),
  ('00000000010', 'Literatura', 2024, 'V'),
  ('00000000010', 'Inglês', 2024, 'V'),
  ('00000000011', 'Português', 2025, 'N'),
  ('00000000011', 'Redação', 2025, 'N'),
  ('00000000010', 'Literatura', 2025, 'N'),
  ('00000000010', 'Inglês', 2025, 'N'),
  
  -- Ciências Humanas
  ('00000000009', 'História do Brasil', 2024, 'V'),
  ('00000000009', 'História Geral', 2024, 'V'),
  ('00000000009', 'Geografia do Brasil', 2024, 'V'),
  ('00000000009', 'Geografia Geral', 2024, 'V'),
  ('00000000012', 'História do Brasil', 2025, 'N'),
  ('00000000012', 'História Geral', 2025, 'N'),
  ('00000000012', 'Geografia do Brasil', 2025, 'N'),
  ('00000000012', 'Geografia Geral', 2025, 'N');

-- =========================
-- TABELA: evento_turma (30)
-- =========================
INSERT INTO evento_turma (evento_id, turma_ano, turma_periodo) VALUES
  -- Simulados 2024 (eventos 1-8) para turmas 2024
  (1, 2024, 'V'), (1, 2024, 'N'),
  (2, 2024, 'V'), (2, 2024, 'N'),
  (3, 2024, 'V'), (3, 2024, 'N'),
  (4, 2024, 'V'), (4, 2024, 'N'),
  (5, 2024, 'V'), (5, 2024, 'N'),
  (6, 2024, 'V'), (6, 2024, 'N'),
  (7, 2024, 'V'), (7, 2024, 'N'),
  (8, 2024, 'V'), (8, 2024, 'N'),
  
  -- Simulados 2025 (eventos 9-15) para turmas 2025
  (9, 2025, 'V'), (9, 2025, 'N'),
  (10, 2025, 'V'), (10, 2025, 'N'),
  (11, 2025, 'V'), (11, 2025, 'N'),
  (12, 2025, 'V'), (12, 2025, 'N'),
  (13, 2025, 'V'), (13, 2025, 'N'),
  (14, 2025, 'V'), (14, 2025, 'N'),
  (15, 2025, 'V'), (15, 2025, 'N'),
  
  -- Aulas 2024 VESPERTINO (eventos 16-25)
  (16, 2024, 'V'), (17, 2024, 'V'), (18, 2024, 'V'), (19, 2024, 'V'), (20, 2024, 'V'),
  (21, 2024, 'V'), (22, 2024, 'V'), (23, 2024, 'V'), (24, 2024, 'V'), (25, 2024, 'V'),
  
  -- Aulas 2024 NOTURNO (eventos 26-35)
  (26, 2024, 'N'), (27, 2024, 'N'), (28, 2024, 'N'), (29, 2024, 'N'), (30, 2024, 'N'),
  (31, 2024, 'N'), (32, 2024, 'N'), (33, 2024, 'N'), (34, 2024, 'N'), (35, 2024, 'N'),
  
  -- Aulas 2025 VESPERTINO (eventos 36-45)
  (36, 2025, 'V'), (37, 2025, 'V'), (38, 2025, 'V'), (39, 2025, 'V'), (40, 2025, 'V'),
  (41, 2025, 'V'), (42, 2025, 'V'), (43, 2025, 'V'), (44, 2025, 'V'), (45, 2025, 'V'),
  
  -- Aulas 2025 NOTURNO (eventos 46-55)
  (46, 2025, 'N'), (47, 2025, 'N'), (48, 2025, 'N'), (49, 2025, 'N'), (50, 2025, 'N'),
  (51, 2025, 'N'), (52, 2025, 'N'), (53, 2025, 'N'), (54, 2025, 'N'), (55, 2025, 'N');

-- =========================
-- TABELA: aluno_evento (30)
-- =========================
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
  -- Simulados 2024 - Alunos da turma 2024-V e 2024-N
  -- ENEM 2024 Dia 1 (evento 1)
  ('00000000011', 1, TRUE),   -- 2024-V
  ('00000000012', 1, FALSE),  -- 2024-N (não matriculado)
  ('00000000013', 1, FALSE),  -- 2024-V (desligado)
  ('00000000014', 1, TRUE),   -- 2024-V
  ('00000000015', 1, TRUE),   -- 2024-N
  
  -- ENEM 2024 Dia 2 (evento 2)
  ('00000000011', 2, TRUE),
  ('00000000014', 2, TRUE),
  ('00000000015', 2, FALSE),  -- Faltou
  
  -- FUVEST 2024 1ª Fase (evento 3)
  ('00000000011', 3, TRUE),
  ('00000000014', 3, TRUE),
  ('00000000015', 3, TRUE),
  
  -- FUVEST 2024 2ª Fase Dia 1 (evento 4)
  ('00000000011', 4, FALSE),  -- Faltou
  ('00000000014', 4, TRUE),
  
  -- FUVEST 2024 2ª Fase Dia 2 (evento 5)
  ('00000000014', 5, TRUE),
  
  -- UNICAMP 2024 1ª Fase (evento 6)
  ('00000000011', 6, TRUE),
  ('00000000014', 6, FALSE),  -- Faltou
  ('00000000015', 6, TRUE),
  
  -- UNICAMP 2024 2ª Fase Dia 1 (evento 7)
  ('00000000011', 7, TRUE),
  ('00000000015', 7, TRUE),
  
  -- UNICAMP 2024 2ª Fase Dia 2 (evento 8)
  ('00000000011', 8, TRUE),
  ('00000000015', 8, FALSE),  -- Faltou
  
  -- Simulados 2025 - Alunos da turma 2025-V e 2025-N
  -- ENEM 2025 Dia 1 (evento 9)
  ('00000000016', 9, TRUE),   -- 2025-N
  ('00000000017', 9, TRUE),   -- 2025-V
  ('00000000018', 9, FALSE),  -- 2025-N (desligado)
  ('00000000020', 9, TRUE),   -- 2025-V
  ('00000000021', 9, FALSE),  -- Faltou
  ('00000000022', 9, TRUE),   -- 2025-V
  ('00000000023', 9, TRUE),   -- 2025-V
  
  -- ENEM 2025 Dia 2 (evento 10)
  ('00000000016', 10, TRUE),
  ('00000000017', 10, TRUE),
  ('00000000019', 10, FALSE), -- Faltou
  ('00000000020', 10, TRUE),
  ('00000000022', 10, TRUE),
  ('00000000023', 10, TRUE),
  
  -- FUVEST 2025 1ª Fase (evento 11)
  ('00000000016', 11, TRUE),
  ('00000000017', 11, TRUE),
  ('00000000020', 11, TRUE),
  ('00000000022', 11, FALSE), -- Faltou
  ('00000000023', 11, TRUE),
  
  -- FUVEST 2025 2ª Fase Dia 1 (evento 12)
  ('00000000016', 12, TRUE),
  ('00000000017', 12, TRUE),
  ('00000000020', 12, TRUE),
  ('00000000023', 12, FALSE), -- Faltou
  
  -- FUVEST 2025 2ª Fase Dia 2 (evento 13)
  ('00000000016', 13, TRUE),
  ('00000000017', 13, FALSE), -- Faltou
  ('00000000020', 13, TRUE),
  
  -- UNICAMP 2025 1ª Fase (evento 14)
  ('00000000016', 14, TRUE),
  ('00000000017', 14, TRUE),
  ('00000000019', 14, FALSE), -- Faltou
  ('00000000020', 14, TRUE),
  ('00000000021', 14, TRUE),
  ('00000000022', 14, TRUE),
  ('00000000023', 14, TRUE),
  
  -- UNICAMP 2025 2ª Fase (evento 15)
  ('00000000016', 15, TRUE),
  ('00000000017', 15, TRUE),
  ('00000000019', 15, FALSE), -- Faltou
  ('00000000020', 15, TRUE),
  ('00000000021', 15, TRUE),
  ('00000000022', 15, TRUE),
  ('00000000023', 15, FALSE); -- Faltou

-- ========================================
-- AULAS 2024 VESPERTINO (eventos 16-25)
-- Alunos: 00000000011, 00000000014 (matriculados)
-- ========================================

-- Aluno 00000000011 (2024-V) - Boa frequência (90%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000011', 16, TRUE),
('00000000011', 17, TRUE),
('00000000011', 18, TRUE),
('00000000011', 19, TRUE),
('00000000011', 20, FALSE), -- Faltou 1
('00000000011', 21, TRUE),
('00000000011', 22, TRUE),
('00000000011', 23, TRUE),
('00000000011', 24, TRUE),
('00000000011', 25, TRUE);

-- Aluno 00000000014 (2024-V) - Frequência média (80%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000014', 16, TRUE),
('00000000014', 17, TRUE),
('00000000014', 18, FALSE), -- Faltou 1
('00000000014', 19, TRUE),
('00000000014', 20, TRUE),
('00000000014', 21, TRUE),
('00000000014', 22, FALSE), -- Faltou 2
('00000000014', 23, TRUE),
('00000000014', 24, TRUE),
('00000000014', 25, TRUE);

-- Aluno 00000000013 (2024-V) - DESLIGADO - Frequência baixa antes de desligar (40%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000013', 16, TRUE),
('00000000013', 17, FALSE),
('00000000013', 18, FALSE),
('00000000013', 19, TRUE),
('00000000013', 20, FALSE),
('00000000013', 21, FALSE),
('00000000013', 22, TRUE),
('00000000013', 23, FALSE),
('00000000013', 24, TRUE),
('00000000013', 25, FALSE);

-- ========================================
-- AULAS 2024 NOTURNO (eventos 26-35)
-- Alunos: 00000000015 (matriculado), 00000000012 (não matriculado)
-- ========================================

-- Aluno 00000000015 (2024-N) - Boa frequência (85%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000015', 26, TRUE),
('00000000015', 27, TRUE),
('00000000015', 28, TRUE),
('00000000015', 29, FALSE), -- Faltou 1
('00000000015', 30, TRUE),
('00000000015', 31, TRUE),
('00000000015', 32, TRUE),
('00000000015', 33, TRUE),
('00000000015', 34, FALSE), -- Faltou 2
('00000000015', 35, TRUE);

-- Aluno 00000000012 (2024-N) - NÃO MATRICULADO - Frequência muito baixa (30%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000012', 26, TRUE),
('00000000012', 27, FALSE),
('00000000012', 28, FALSE),
('00000000012', 29, TRUE),
('00000000012', 30, FALSE),
('00000000012', 31, FALSE),
('00000000012', 32, TRUE),
('00000000012', 33, FALSE),
('00000000012', 34, FALSE),
('00000000012', 35, FALSE);

-- ========================================
-- AULAS 2025 VESPERTINO (eventos 36-45)
-- Alunos: 00000000017, 00000000020, 00000000022, 00000000023 (todos matriculados)
-- ========================================

-- Aluno 00000000017 (2025-V) - Excelente frequência (95%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000017', 36, TRUE),
('00000000017', 37, TRUE),
('00000000017', 38, TRUE),
('00000000017', 39, TRUE),
('00000000017', 40, TRUE),
('00000000017', 41, TRUE),
('00000000017', 42, TRUE),
('00000000017', 43, TRUE),
('00000000017', 44, FALSE), -- Faltou 1
('00000000017', 45, TRUE);

-- Aluno 00000000020 (2025-V) - Excelente frequência (100%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000020', 36, TRUE),
('00000000020', 37, TRUE),
('00000000020', 38, TRUE),
('00000000020', 39, TRUE),
('00000000020', 40, TRUE),
('00000000020', 41, TRUE),
('00000000020', 42, TRUE),
('00000000020', 43, TRUE),
('00000000020', 44, TRUE),
('00000000020', 45, TRUE);

-- Aluno 00000000022 (2025-V) - Boa frequência (85%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000022', 36, TRUE),
('00000000022', 37, TRUE),
('00000000022', 38, FALSE), -- Faltou 1
('00000000022', 39, TRUE),
('00000000022', 40, TRUE),
('00000000022', 41, TRUE),
('00000000022', 42, TRUE),
('00000000022', 43, FALSE), -- Faltou 2
('00000000022', 44, TRUE),
('00000000022', 45, TRUE);

-- Aluno 00000000023 (2025-V) - Frequência média (80%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000023', 36, TRUE),
('00000000023', 37, FALSE), -- Faltou 1
('00000000023', 38, TRUE),
('00000000023', 39, TRUE),
('00000000023', 40, TRUE),
('00000000023', 41, FALSE), -- Faltou 2
('00000000023', 42, TRUE),
('00000000023', 43, TRUE),
('00000000023', 44, TRUE),
('00000000023', 45, TRUE);

-- ========================================
-- AULAS 2025 NOTURNO (eventos 46-55)
-- Alunos: 00000000016, 00000000019, 00000000021 (matriculados)
--         00000000018 (desligado)
-- ========================================

-- Aluno 00000000016 (2025-N) - Excelente frequência (95%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000016', 46, TRUE),
('00000000016', 47, TRUE),
('00000000016', 48, TRUE),
('00000000016', 49, TRUE),
('00000000016', 50, FALSE), -- Faltou 1
('00000000016', 51, TRUE),
('00000000016', 52, TRUE),
('00000000016', 53, TRUE),
('00000000016', 54, TRUE),
('00000000016', 55, TRUE);

-- Aluno 00000000019 (2025-N) - Boa frequência (90%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000019', 46, TRUE),
('00000000019', 47, TRUE),
('00000000019', 48, TRUE),
('00000000019', 49, FALSE), -- Faltou 1
('00000000019', 50, TRUE),
('00000000019', 51, TRUE),
('00000000019', 52, TRUE),
('00000000019', 53, TRUE),
('00000000019', 54, TRUE),
('00000000019', 55, TRUE);

-- Aluno 00000000021 (2025-N) - Frequência média (75%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000021', 46, TRUE),
('00000000021', 47, FALSE), -- Faltou 1
('00000000021', 48, TRUE),
('00000000021', 49, TRUE),
('00000000021', 50, FALSE), -- Faltou 2
('00000000021', 51, TRUE),
('00000000021', 52, TRUE),
('00000000021', 53, FALSE), -- Faltou 3
('00000000021', 54, TRUE),
('00000000021', 55, TRUE);

-- Aluno 00000000018 (2025-N) - DESLIGADO - Frequência baixa (50%)
INSERT INTO aluno_evento (aluno_cpf, evento_id, presente) VALUES
('00000000018', 46, TRUE),
('00000000018', 47, FALSE),
('00000000018', 48, TRUE),
('00000000018', 49, FALSE),
('00000000018', 50, TRUE),
('00000000018', 51, FALSE),
('00000000018', 52, TRUE),
('00000000018', 53, FALSE),
('00000000018', 54, TRUE),
('00000000018', 55, FALSE);

-- =========================
-- TABELA: questao_materia (30)
-- =========================
INSERT INTO questao_materia (questao_prova_id, questao_numero, materia_nome, subarea) VALUES
-- ENEM 2024 Dia 1 - Linguagens e Humanas
  (1, 1, 'Português', 'Interpretação de Texto'),
  (1, 2, 'História do Brasil', 'Brasil Colônia'),
  (1, 3, 'Geografia do Brasil', 'Urbanização'),
  
  -- ENEM 2024 Dia 2 - Natureza e Matemática
  (2, 1, 'Álgebra', 'Equações do 2º Grau'),
  (2, 2, 'Química Orgânica', 'Reações Orgânicas'),
  (2, 3, 'Física', 'Cinemática'),
  
  -- FUVEST 2024 - 1ª Fase
  (3, 1, 'Literatura', 'Realismo Brasileiro'),
  (3, 2, 'História Geral', 'Revolução Francesa'),
  (3, 3, 'Álgebra', 'Probabilidade'),
  
  -- FUVEST 2024 - 2ª Fase Dia 1 (Português)
  (4, 1, 'Português', 'Dissertação'),
  (4, 2, 'Português', 'Regência Verbal'),
  (4, 3, 'Redação', 'Texto Argumentativo'),
  
  -- FUVEST 2024 - 2ª Fase Dia 2 (Exatas)
  (5, 1, 'Geometria', 'Geometria Analítica'),
  (5, 2, 'Física', 'Termodinâmica'),
  (5, 3, 'Química Inorgânica', 'Equilíbrio Químico'),
  
  -- UNICAMP 2024 - 1ª Fase
  (6, 1, 'História do Brasil', 'Movimentos Sociais'),
  (6, 2, 'Literatura', 'Modernismo'),
  (6, 3, 'Biologia', 'Ecologia'),
  
  -- UNICAMP 2024 - 2ª Fase Dia 1
  (7, 1, 'Literatura', 'Romance Regional'),
  (7, 2, 'História do Brasil', 'Redemocratização'),
  (7, 3, 'Geografia do Brasil', 'Questão Agrária'),
  
  -- UNICAMP 2024 - 2ª Fase Dia 2
  (8, 1, 'Álgebra', 'Sistemas Lineares'),
  (8, 2, 'Física', 'Ondas Eletromagnéticas'),
  (8, 3, 'Química Orgânica', 'Polímeros'),
  
  -- ENEM 2025 Dia 1
  (9, 1, 'Português', 'Interpretação de Charge'),
  (9, 2, 'Atualidades', 'Ética e Moral'),
  (9, 3, 'Atualidades', 'Cultura Urbana'),
  
  -- ENEM 2025 Dia 2
  (10, 1, 'Álgebra', 'Estatística'),
  (10, 2, 'Biologia', 'Genética'),
  (10, 3, 'Química Inorgânica', 'Oxirredução'),
  
  -- FUVEST 2025 - 1ª Fase
  (11, 1, 'Literatura', 'Crônica'),
  (11, 2, 'História do Brasil', 'Era Vargas'),
  (11, 3, 'Álgebra', 'Funções'),
  
  -- FUVEST 2025 - 2ª Fase Dia 1
  (12, 1, 'Português', 'Análise Sintática'),
  (12, 2, 'Literatura', 'Romantismo e Realismo'),
  (12, 3, 'Redação', 'Desigualdade Social'),
  
  -- FUVEST 2025 - 2ª Fase Dia 2
  (13, 1, 'Geometria', 'Trigonometria'),
  (13, 2, 'Física', 'Eletromagnetismo'),
  (13, 3, 'Química Inorgânica', 'Cinética Química'),
  
  -- UNICAMP 2025 - 1ª Fase
  (14, 1, 'Geografia Geral', 'Mudanças Climáticas'),
  (14, 2, 'História Geral', 'Colonialismo'),
  (14, 3, 'Biologia', 'Evolução'),
  
  -- UNICAMP 2025 - 2ª Fase
  (15, 1, 'Física', 'Energia Renovável'),
  (15, 2, 'Álgebra', 'Modelagem Matemática'),
  (15, 3, 'Redação', 'Educação no Brasil');

-- =========================
-- TABELA: aluno_questao (CORRIGIDO - respostas objetivas)
-- =========================
INSERT INTO aluno_questao (aluno_cpf, questao_prova_id, questao_numero, tempo_resposta_seg, alternativa) VALUES
  -- ENEM 2024 Dia 1 - Alunos presentes: 00000000011, 00000000014, 00000000015
  ('00000000011', 1, 1, 120, 'A'),  -- Gabarito: A (CORRETO)
  ('00000000011', 1, 2, 150, 'C'),  -- Gabarito: C (CORRETO)
  -- ('00000000011', 1, 3) -- EM BRANCO
  
  ('00000000014', 1, 1, 110, 'A'),  -- CORRETO
  ('00000000014', 1, 2, 140, 'B'),  -- ERRADO (gabarito: C)
  -- ('00000000014', 1, 3) -- EM BRANCO
  
  ('00000000015', 1, 1, 130, 'B'),  -- ERRADO (gabarito: A)
  ('00000000015', 1, 2, 155, 'C'),  -- CORRETO
  -- ('00000000015', 1, 3) -- EM BRANCO
  
  -- ENEM 2024 Dia 2 - Alunos presentes: 00000000011, 00000000014
  ('00000000011', 2, 1, 135, 'B'),  -- Gabarito: B (CORRETO)
  ('00000000011', 2, 2, 145, 'E'),  -- Gabarito: E (CORRETO)
  ('00000000011', 2, 3, 155, 'A'),  -- Gabarito: A (CORRETO)
  
  ('00000000014', 2, 1, 125, 'B'),  -- CORRETO
  ('00000000014', 2, 2, 160, 'D'),  -- ERRADO (gabarito: E)
  ('00000000014', 2, 3, 140, 'A'),  -- CORRETO
  
  -- FUVEST 2024 1ª Fase - Alunos presentes: 00000000011, 00000000014, 00000000015
  ('00000000011', 3, 1, 200, 'C'),  -- Gabarito: C (CORRETO)
  ('00000000011', 3, 2, 180, 'B'),  -- Gabarito: B (CORRETO)
  ('00000000011', 3, 3, 220, 'D'),  -- Gabarito: D (CORRETO)
  
  ('00000000014', 3, 1, 190, 'C'),  -- CORRETO
  ('00000000014', 3, 2, 170, 'A'),  -- ERRADO (gabarito: B)
  ('00000000014', 3, 3, 210, 'D'),  -- CORRETO
  
  ('00000000015', 3, 1, 210, 'B'),  -- ERRADO (gabarito: C)
  ('00000000015', 3, 2, 185, 'B'),  -- CORRETO
  ('00000000015', 3, 3, 195, 'C'),  -- ERRADO (gabarito: D)
  
  -- FUVEST 2024 2ª Fase Dia 1 - Aluno presente: 00000000014
  -- Gabaritos: B, D, A
  ('00000000014', 4, 1, 300, 'B'),  -- CORRETO
  ('00000000014', 4, 2, 250, 'C'),  -- ERRADO (gabarito: D)
  ('00000000014', 4, 3, 1800, 'A'), -- CORRETO
  
  -- FUVEST 2024 2ª Fase Dia 2 - Aluno presente: 00000000014
  -- Gabaritos: C, E, B
  ('00000000014', 5, 1, 400, 'C'),  -- CORRETO
  -- ('00000000014', 5, 2) -- EM BRANCO
  ('00000000014', 5, 3, 380, 'B'),  -- CORRETO
  
  -- UNICAMP 2024 1ª Fase - Alunos presentes: 00000000011, 00000000015
  ('00000000011', 6, 1, 165, 'B'),  -- Gabarito: B (CORRETO)
  ('00000000011', 6, 2, 175, 'D'),  -- Gabarito: D (CORRETO)
  ('00000000011', 6, 3, 185, 'A'),  -- Gabarito: A (CORRETO)
  
  -- Aluno 00000000015 deixou TUDO em branco
  
  -- UNICAMP 2024 2ª Fase Dia 1 - Alunos presentes: 00000000011, 00000000015
  -- Gabaritos: A, C, E
  ('00000000011', 7, 1, 450, 'A'),  -- CORRETO
  ('00000000011', 7, 2, 400, 'C'),  -- CORRETO
  ('00000000011', 7, 3, 420, 'E'),  -- CORRETO
  
  ('00000000015', 7, 1, 480, 'B'),  -- ERRADO (gabarito: A)
  ('00000000015', 7, 2, 410, 'C'),  -- CORRETO
  -- ('00000000015', 7, 3) -- EM BRANCO
  
  -- UNICAMP 2024 2ª Fase Dia 2 - Aluno presente: 00000000011
  -- Gabaritos: D, B, A
  ('00000000011', 8, 1, 500, 'D'),  -- CORRETO
  ('00000000011', 8, 2, 470, 'B'),  -- CORRETO
  ('00000000011', 8, 3, 490, 'A'),  -- CORRETO
  
  -- ENEM 2025 Dia 1 - Alunos presentes: 00000000016, 00000000017, 00000000019, 00000000020, 00000000022, 00000000023
  ('00000000016', 9, 1, 125, 'C'),  -- Gabarito: C (CORRETO)
  ('00000000016', 9, 2, 135, 'B'),  -- Gabarito: B (CORRETO)
  ('00000000016', 9, 3, 145, 'E'),  -- Gabarito: E (CORRETO)
  
  ('00000000017', 9, 1, 130, 'C'),  -- CORRETO
  -- ('00000000017', 9, 2) -- EM BRANCO
  -- ('00000000017', 9, 3) -- EM BRANCO
  
  -- ('00000000019', 9, 1, 120, 'B'),  -- ERRADO (gabarito: C)
  -- ('00000000019', 9, 2) -- EM BRANCO
  -- ('00000000019', 9, 3) -- EM BRANCO
  
  ('00000000020', 9, 1, 128, 'C'),  -- CORRETO
  ('00000000020', 9, 2, 142, 'B'),  -- CORRETO
  ('00000000020', 9, 3, 152, 'E'),  -- CORRETO
  
  ('00000000022', 9, 1, 132, 'C'),  -- CORRETO
  ('00000000022', 9, 2, 136, 'C'),  -- ERRADO (gabarito: B)
  ('00000000022', 9, 3, 146, 'E'),  -- CORRETO
  
  ('00000000023', 9, 1, 127, 'C'),  -- CORRETO
  ('00000000023', 9, 2, 139, 'B'),  -- CORRETO
  ('00000000023', 9, 3, 149, 'A'),  -- ERRADO (gabarito: E)
  
  -- ENEM 2025 Dia 2 - Alunos presentes: 00000000016, 00000000017, 00000000020, 00000000022, 00000000023
  ('00000000016', 10, 1, 155, 'D'),  -- Gabarito: D (CORRETO)
  ('00000000016', 10, 2, 165, 'A'),  -- Gabarito: A (CORRETO)
  ('00000000016', 10, 3, 175, 'C'),  -- Gabarito: C (CORRETO)
  
  ('00000000017', 10, 1, 160, 'D'),  -- CORRETO
  ('00000000017', 10, 2, 170, 'B'),  -- ERRADO (gabarito: A)
  ('00000000017', 10, 3, 180, 'C'),  -- CORRETO
  
  ('00000000020', 10, 1, 150, 'D'),  -- CORRETO
  ('00000000020', 10, 2, 162, 'A'),  -- CORRETO
  ('00000000020', 10, 3, 172, 'C'),  -- CORRETO
  
  ('00000000022', 10, 1, 158, 'C'),  -- ERRADO (gabarito: D)
  -- ('00000000022', 10, 2) -- EM BRANCO
  ('00000000022', 10, 3, 178, 'B'),  -- ERRADO (gabarito: C)
  
  ('00000000023', 10, 1, 153, 'D'),  -- CORRETO
  ('00000000023', 10, 2, 163, 'A'),  -- CORRETO
  ('00000000023', 10, 3, 173, 'C'),  -- CORRETO
  
  -- FUVEST 2025 1ª Fase - Alunos presentes: 00000000016, 00000000017, 00000000020, 00000000023
  ('00000000016', 11, 1, 195, 'B'),  -- Gabarito: B (CORRETO)
  ('00000000016', 11, 2, 205, 'D'),  -- Gabarito: D (CORRETO)
  ('00000000016', 11, 3, 215, 'A'),  -- Gabarito: A (CORRETO)
  
  ('00000000017', 11, 1, 200, 'B'),  -- CORRETO
  ('00000000017', 11, 2, 210, 'C'),  -- ERRADO (gabarito: D)
  ('00000000017', 11, 3, 220, 'A'),  -- CORRETO
  
  ('00000000020', 11, 1, 190, 'B'),  -- CORRETO
  -- ('00000000020', 11, 2) -- EM BRANCO
  -- ('00000000020', 11, 3) -- EM BRANCO
  
  ('00000000023', 11, 1, 198, 'A'),  -- ERRADO (gabarito: B)
  ('00000000023', 11, 2, 208, 'D'),  -- CORRETO
  ('00000000023', 11, 3, 218, 'A'),  -- CORRETO
  
  -- FUVEST 2025 2ª Fase Dia 1 - Alunos presentes: 00000000016, 00000000017, 00000000020
  -- Gabaritos: C, E, B
  ('00000000016', 12, 1, 310, 'C'),  -- CORRETO
  ('00000000016', 12, 2, 280, 'E'),  -- CORRETO
  ('00000000016', 12, 3, 1900, 'B'), -- CORRETO
  
  ('00000000017', 12, 1, 320, 'C'),  -- CORRETO
  ('00000000017', 12, 2, 290, 'D'),  -- ERRADO (gabarito: E)
  ('00000000017', 12, 3, 1850, 'B'), -- CORRETO
  
  ('00000000020', 12, 1, 305, 'A'),  -- ERRADO (gabarito: C)
  ('00000000020', 12, 2, 285, 'E'),  -- CORRETO
  -- ('00000000020', 12, 3) -- EM BRANCO
  
  -- FUVEST 2025 2ª Fase Dia 2 - Alunos presentes: 00000000016, 00000000020
  -- Gabaritos: A, D, C
  ('00000000016', 13, 1, 420, 'A'),  -- CORRETO
  ('00000000016', 13, 2, 390, 'D'),  -- CORRETO
  ('00000000016', 13, 3, 410, 'C'),  -- CORRETO
  
  ('00000000020', 13, 1, 430, 'B'),  -- ERRADO (gabarito: A)
  ('00000000020', 13, 2, 395, 'D'),  -- CORRETO
  ('00000000020', 13, 3, 415, 'C'),  -- CORRETO
  
  -- UNICAMP 2025 1ª Fase - Alunos presentes: 00000000016, 00000000017, 00000000019, 00000000020, 00000000021, 00000000022, 00000000023
  ('00000000016', 14, 1, 175, 'C'),  -- Gabarito: C (CORRETO)
  ('00000000016', 14, 2, 185, 'E'),  -- Gabarito: E (CORRETO)
  ('00000000016', 14, 3, 195, 'B'),  -- Gabarito: B (CORRETO)
  
  ('00000000017', 14, 1, 180, 'C'),  -- CORRETO
  ('00000000017', 14, 2, 190, 'D'),  -- ERRADO (gabarito: E)
  ('00000000017', 14, 3, 200, 'B'),  -- CORRETO
  
  ('00000000019', 14, 1, 170, 'B'),  -- ERRADO (gabarito: C)
  ('00000000019', 14, 2, 188, 'E'),  -- CORRETO
  ('00000000019', 14, 3, 198, 'A'),  -- ERRADO (gabarito: B)
  
  ('00000000020', 14, 1, 178, 'C'),  -- CORRETO
  ('00000000020', 14, 2, 186, 'E'),  -- CORRETO
  ('00000000020', 14, 3, 196, 'B'),  -- CORRETO
  
  ('00000000021', 14, 1, 182, 'C'),  -- CORRETO
  ('00000000021', 14, 2, 192, 'A'),  -- ERRADO (gabarito: E)
  ('00000000021', 14, 3, 202, 'B'),  -- CORRETO
  
  ('00000000022', 14, 1, 176, 'A'),  -- ERRADO (gabarito: C)
  ('00000000022', 14, 2, 184, 'E'),  -- CORRETO
  ('00000000022', 14, 3, 194, 'C'),  -- ERRADO (gabarito: B)
  
  ('00000000023', 14, 1, 179, 'C'),  -- CORRETO
  ('00000000023', 14, 2, 189, 'E'),  -- CORRETO
  ('00000000023', 14, 3, 199, 'B'),  -- CORRETO
  
  -- UNICAMP 2025 2ª Fase - Alunos presentes: 00000000016, 00000000017, 00000000020, 00000000021, 00000000022
  -- Gabaritos: B, D, A
  ('00000000016', 15, 1, 510, 'B'),  -- CORRETO
  ('00000000016', 15, 2, 480, 'D'),  -- CORRETO
  ('00000000016', 15, 3, 1950, 'A'), -- CORRETO
  
  ('00000000017', 15, 1, 520, 'B'),  -- CORRETO
  ('00000000017', 15, 2, 490, 'C'),  -- ERRADO (gabarito: D)
  ('00000000017', 15, 3, 2000, 'A'), -- CORRETO
  
  ('00000000020', 15, 1, 505, 'A'),  -- ERRADO (gabarito: B)
  ('00000000020', 15, 2, 485, 'D'),  -- CORRETO
  -- ('00000000020', 15, 3) -- EM BRANCO
  
  ('00000000021', 15, 1, 515, 'B'),  -- CORRETO
  ('00000000021', 15, 2, 495, 'E'),  -- ERRADO (gabarito: D)
  ('00000000021', 15, 3, 2010, 'A'), -- CORRETO
  
  ('00000000022', 15, 1, 500, 'C'),  -- ERRADO (gabarito: B)
  ('00000000022', 15, 2, 475, 'D'),  -- CORRETO
  ('00000000022', 15, 3, 1970, 'B'); -- ERRADO (gabarito: A)