# 📅 Sistema de Gerenciamento de Espaços

Sistema web desenvolvido em **ASP.NET Core MVC** para gerenciamento de espaços (salas, laboratórios, coworkings), usuários e reservas, com controle de disponibilidade, cálculo automático de valores e geração de relatórios.

---

## 🎯 Objetivo

Permitir o gerenciamento completo de espaços e reservas, garantindo controle de horários, integridade dos dados e visualização clara das informações.

---

## 🛠 Tecnologias Utilizadas

- ASP.NET Core MVC  
- Entity Framework Core  
- SQL Server  
- ASP.NET Identity  
- Tailwind CSS  
- Chart.js  
- DataTables  
- QuestPDF  

---

## 📂 Funcionalidades

### 👤 Usuários
- Cadastro e autenticação
- Login e logout
- Associação de reservas ao usuário

### 🏢 Espaços
- Cadastro de espaços
- Listagem, edição e exclusão
- Associação automática com reservas
⚠️ Um espaço não pode ser excluído se possuir reservas associadas, pois o banco de dados possui Foreign Key garantindo a integridade.

### 📆 Reservas
- Criação de reservas com data e hora
- Validação de conflitos de horário
- Atualização automática do status:
  - **Ativa**
  - **Finalizada**
- Edição de datas com recálculo automático do valor
- Exclusão de reservas


## 💰 Cálculo do Valor da Reserva

O valor da reserva é calculado automaticamente com base:

- Na diferença entre **DataInicio** e **DataFim**
- No valor configurado do espaço

Não é utilizado valor fixo por hora.  
O valor é recalculado sempre que a reserva é editada.

---

## 📊 Dashboard

- Total de reservas ativas
- Total de reservas finalizadas
- Quantidade de reservas por espaço
- Visualização gráfica com Chart.js

---

## 📄 Geração de PDF

Cada reserva pode gerar um comprovante em PDF contendo:

- Espaço reservado
- Usuário
- Data e hora de início e fim
- Situação da reserva
- Valor total

A geração do PDF é feita no backend utilizando **QuestPDF**.

---

## 🔒 Integridade dos Dados

O sistema utiliza **restrições de integridade referencial (Foreign Key)** no banco de dados.

### Exclusão de Espaços

- Não é permitido excluir um espaço que possua reservas associadas
- A regra é garantida diretamente pelo SQL Server
- Não há validação manual no código para essa regra

Essa abordagem garante:
- Consistência dos dados
- Segurança
- Centralização da regra no banco

---

## 📱 Responsividade

O sistema é totalmente responsivo em Desktop, mas a responsivadade nos celurares não está 100%:

- Desktop
- Tablets
- Smartphones (Android e iOS)

A responsividade é garantida com **Tailwind CSS**.

---

## 🧪 Validações Implementadas

- Não permite reservas em datas passadas
- Data final deve ser maior que a inicial
- Não permite conflito de horários no mesmo espaço
- Valor recalculado automaticamente ao editar a reserva

---

## 🚀 Considerações Finais

O sistema segue boas práticas de desenvolvimento, utilizando arquitetura MVC, separação de responsabilidades e garantindo integridade dos dados, atendendo aos requisitos funcionais e técnicos propostos.

---

## 👨‍💻 Autor
Matheus viana Guimarães Lima
Projeto desenvolvido para fins acadêmicos.
