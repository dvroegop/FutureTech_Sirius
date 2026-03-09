# Sirius Cybernetics Complaint Handling System
## (Entirely, but not quite, totally unlike a service-oriented architecture)

Welcome, new employee of the **Sirius Cybernetics Corporation**.

Share and Enjoy.

You are now responsible for maintaining the complaint handling system used by the Sirius Cybernetics Corporation — the same department whose complaint files currently occupy the major landmass of **three moderately sized planets**.

Naturally, the system is powered by modern software architecture, deterministic logic, and a carefully contained amount of artificial intelligence. Because allowing AI to run your entire customer support department unattended would be *deeply irresponsible*.

This repository contains the reference implementation used in the workshop:

**Architecting AI in .NET**

---

# The Problem

Modern software systems are deterministic.

They behave in predictable ways:

input -> logic -> output

LLMs are **not deterministic**.

They behave more like this:

input -> probabilistic reasoning -> "probably correct" output

Mix those two without thinking, and you get a system that behaves roughly like a depressed robot with an oversized ego and a marketing department.

Which is, coincidentally, how most AI integrations are currently built.

Typical bad implementations look like this:

- Business logic hidden in prompts
- LLM directly calling APIs
- No validation
- No failure handling
- No boundaries

The result:

**Architectural chaos.**

This project demonstrates how to do it **properly**.

---

# Architecture Overview

The system separates deterministic and probabilistic behavior.

Application API  
↓  
AI Service Layer  
↓  
LLM  
↓  
MCP Server Tools

Important rule:

**The LLM never executes business logic.**

It only decides **which tool should be used**.

The actual work happens inside deterministic services.

This prevents the system from doing things like:

- refunding random customers
- closing tickets that were never opened
- starting interstellar wars

---

# Core Components

## API

The API acts as the deterministic control layer.

Responsibilities:

- receiving user input
- invoking the AI service
- validating results
- executing tools

Think of it as the **adult in the room**.

---

## AI Service

The AI service is responsible for interacting with the LLM.

Responsibilities:

- constructing prompts
- enforcing structured output
- defining available tools
- handling retries and failures

Important rule:

**No business rules in prompts.**

Prompts describe the task.

The system enforces the rules.

---

## LLM

The LLM acts as a **reasoning engine**.

It determines:

- whether a complaint requires a ticket
- whether a refund might be appropriate
- which tool should be called

The LLM **does not execute actions**.

It merely suggests them.

Much like a consultant.

---

## MCP Server

Tools are implemented inside an **MCP Server**.

Example tools:

### CreateTicket

CreateTicket(customerName: string, description: string)

Creates a new complaint ticket.

### IssueRefund

IssueRefund(customerName: string, amount: number, description: string)

Issues a refund to a customer.

Subject to approval by several departments and at least one slightly annoyed bureaucrat.

---

# Structured Output

LLM responses are required to return **structured JSON**.

Example:

{
  "isRefundSuggested": true,
  "reasonForDecision": "Customer received a robot that is actively insulting them."
}

This ensures that:

- responses are machine-readable
- decisions can be validated
- hallucinations are contained

Because while hallucinations can be charming in art, they are somewhat less charming in financial systems.

---

# Guardrails

The system includes several safeguards.

### Validation

All AI responses are validated before execution.

### Observability

We track:

- token usage
- tool calls
- response structure
- errors

### Boundaries

The LLM cannot:

- access databases
- execute code
- call APIs directly

It can only **suggest tool calls**.

This keeps the system predictable.

And prevents the complaints department from accidentally colonizing another planet.

---

# Running the System

Start the MCP Server:

dotnet run --project MCPServer

Start the API:

dotnet run --project TicketSystem.Api

Send a complaint.

Observe the system calmly and rationally process the request.

Marvel at the fact that this is already more competent than most customer support departments in the galaxy.

---

# Workshop Goals

During the workshop participants will:

- analyze a **bad AI architecture**
- refactor it into a **proper architecture**
- introduce boundaries between deterministic and probabilistic systems
- implement **MCP tools**
- enforce **structured outputs**

The goal is simple:

**Use AI without destroying your architecture.**

---

# Final Advice

When integrating AI into your architecture, always remember:

Artificial intelligence is incredibly powerful.

Unfortunately, so is artificial stupidity.

Design your systems accordingly.

---

**Sirius Cybernetics Corporation**

Share and Enjoy.