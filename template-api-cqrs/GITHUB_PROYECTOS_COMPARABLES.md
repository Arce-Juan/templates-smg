# Proyectos en GitHub comparables (según JD Censys y entrevista técnica)

Criterios de búsqueda alineados con:

- **Documentación Censys:** `C:\Users\juana\Documents\JUAN\Personal\Censys` (entrevista-tecnica.md, ARQUITECTURA.md, ejemplo-challenge).
- **Entrevista técnica:** Backend financiero con procesos asincrónicos; proceso de 5 pasos; eventos; background; componentes desacoplados; estados; manejo de fallos.
- **Requisitos para esta lista:**  
  - **Mismo proceso de evaluación:** Registrar solicitud → Validar elegibilidad → Evaluar riesgo → Calcular condiciones → Emitir decisión (o equivalente muy cercano).  
  - **Comunicación por eventos** y/o procesos en background.  
  - **C#** (.NET).  
  - **Última actualización:** hace **al menos 10 días** (repos no actualizados en los últimos 10 días, para priorizar proyectos asentados).

---

## Referencia: qué pide la entrevista técnica

- **Proceso de negocio (5 pasos):**  
  1. Registrar la solicitud  
  2. Validar elegibilidad del cliente  
  3. Evaluar riesgo crediticio  
  4. Calcular condiciones del crédito  
  5. Emitir una decisión final  

- **Cada paso:** asincrónico, puede fallar, impacta en el estado. **No** transaccional de punta a punta.  
- **Arquitectura:** procesos en **background**, comunicación **basada en eventos**, componentes **desacoplados**.  
- **Estados:** ej. Creado, En Progreso, Aprobado, Reejecutado, Con Falla.  
- **API mínima:** crear solicitud + consultar estado.

Los proyectos que **cumplen al 100%** este enunciado suelen ser entregas de la misma prueba (p. ej. vuestro **credit-system** y el **Censys-Credit-System** del otro candidato). En GitHub es raro encontrar un repo público que replique exactamente los 5 pasos + eventos + C#.

---

## 1. C# y proceso muy similar (elegibilidad → riesgo → condiciones → decisión)

### **mrfiliputti/credit-eligibility-durable-functions**

- **URL:** https://github.com/mrfiliputti/credit-eligibility-durable-functions  
- **Descripción:** Workflow para evaluar elegibilidad de crédito del cliente mediante **orquestador** y **actividades**; endpoint de estado personalizado.  
- **Proceso:** 3 pasos orquestados: (1) FetchCustomerProfile, (2) CalculateDebtToIncome, (3) DetermineEligibility. Reglas: score mínimo 650, DTI máx 40%, monto ≤ 50% ingreso.  
- **Stack:** C#, .NET 9, **Azure Durable Functions** (orquestación y actividades), HTTP trigger para iniciar y GET para estado.  
- **Eventos / async:** Sí: orquestación durable, actividades async, estado consultable. No usa RabbitMQ/MassTransit; usa el motor de Durable Functions.  
- **Última actualización:** Commits sept 2025 (muy por encima de 10 días).  
- **Alineación con la JD:** Mismo tipo de problema (solicitud de crédito, pasos, estado, async). Menos pasos (3) y tecnología distinta (Azure vs RabbitMQ/Worker). Buen referente de **diseño lógico** y flujo.

---

### **marcin-sieminski/CreditApplications**

- **URL:** https://github.com/marcin-sieminski/CreditApplications  
- **Descripción:** “Credit Applications – workflow system for financial institutions” (ASP.NET Core MVC, Web API, Xamarin, SQL Server).  
- **Proceso:** Workflow de solicitudes de crédito para entidades financieras; no está documentado en el README si son exactamente los 5 pasos (elegibilidad → riesgo → condiciones → decisión), pero el dominio es el mismo.  
- **Stack:** C#, ASP.NET Core, Web API, SQL Server, Xamarin (cliente móvil).  
- **Eventos / background:** Arquitectura por capas y flujo de trabajo; habría que revisar el código para ver si usa eventos o colas.  
- **Última actualización:** Oct 2023 (muy por encima de 10 días).  
- **Alineación con la JD:** Mejor referente **.NET mismo dominio** (solicitudes de crédito, instituciones financieras). Útil para comparar estructura y capas.

---

### **spiritomtom/Petko-Dapchev-Credit**

- **URL:** https://github.com/spiritomtom/Petko-Dapchev-Credit  
- **Descripción:** “A web app that takes care of credit request”.  
- **Stack:** C#.  
- **Última actualización:** Ago 2025 (muy por encima de 10 días).  
- **Alineación con la JD:** Nombre y propósito alineados; no se ha verificado en el código si implementa los 5 pasos y eventos. Candidato a revisar manualmente.

---

### **JoaoSimino/CreditScoringEngine**

- **URL:** https://github.com/JoaoSimino/CreditScoringEngine  
- **Descripción:** Microservicio que procesa **propuestas de crédito**: cruza datos del cliente y monto solicitado, calcula **Internal Score** y genera justificación de aprobación/rechazo.  
- **Stack:** C#, Docker, CI.  
- **Proceso:** Un servicio de scoring/decisión; podría ser **un paso** (p. ej. “evaluar riesgo” o “emitir decisión”) dentro de un flujo mayor. No está documentado un pipeline de 5 pasos con eventos.  
- **Última actualización:** Jul 2025 (muy por encima de 10 días).  
- **Alineación con la JD:** Relacionado con la entrevista (evaluación de crédito, aprobación/rechazo). Referente de **un paso** del proceso o de integración con otros servicios.

---

## 2. C# y contexto financiero / crédito (proceso no verificado como 5 pasos + eventos)

### **Rozagle/FibaBank_**

- **URL:** https://github.com/Rozagle/FibaBank_  
- **Descripción:** Aplicación bancaria full-stack: inversiones, gestión de tarjetas de crédito, transacciones, arquitectura MVC.  
- **Stack:** C#, Entity Framework Core, SQL Server.  
- **Última actualización:** Feb 2026 en API (si se interpreta como 2025, ~10 días atrás).  
- **Alineación con la JD:** Banca y crédito en .NET; no está documentado un flujo de 5 pasos con eventos. Útil para comparar estilo .NET y dominio financiero.

---

### **JPauloSilvaDev/CreditSystem**

- **URL:** https://github.com/JPauloSilvaDev/CreditSystem  
- **Descripción:** Plataforma de análisis de crédito / gestión de clientes inadimplentes: deudas, pagos, registro de clientes y deudores.  
- **Stack:** C#, .NET 9 MVC, SQL Server, SOLID, Clean Code.  
- **Última actualización:** Por revisar en el repo.  
- **Alineación con la JD:** Dominio crédito/gestión; no es el mismo proceso (alta de solicitud → elegibilidad → riesgo → condiciones → decisión). Referente de arquitectura .NET en contexto crédito.

---

## 3. No C# pero proceso muy alineado (para contexto)

- **Omkie-111/credit_approval_system** (Django, Celery, PostgreSQL): registro → elegibilidad → préstamo → consulta; async con Celery.  
- **nagornuuu/Loan-Approval-System** (Java): aprobación de préstamos por score, ingresos, deudas.  
- **eliasnogueira/credit-api** (Java, Spring Boot): API de simulación de crédito; útil para contratos y tests.

Estos no cumplen “C#” pero sirven para comparar **diseño del proceso** (pasos, async, estados).

---

## 4. Resumen: qué usar para comparar (C# + proceso tipo JD + última actualización ≥ 10 días)

| Repositorio | C# | 5 pasos (o muy similar) | Eventos / background | Últ. act. ≥ 10 días | Comentario |
|-------------|-----|--------------------------|----------------------|----------------------|------------|
| **mrfiliputti/credit-eligibility-durable-functions** | Sí | 3 pasos (elegibilidad/DTI/decisión) | Sí (Durable Functions) | Sí | Mejor candidato público: mismo tipo de problema, C#, async, estado. |
| **marcin-sieminski/CreditApplications** | Sí | Workflow crédito (detalle en código) | Por revisar | Sí | Mejor referente .NET mismo dominio. |
| **spiritomtom/Petko-Dapchev-Credit** | Sí | Por revisar | Por revisar | Sí | “Credit request” en C#; revisar si tiene pasos y eventos. |
| **JoaoSimino/CreditScoringEngine** | Sí | Un paso (scoring/decisión) | Microservicio | Sí | Un eslabón del proceso; referente de integración. |
| **Rozagle/FibaBank_** | Sí | No documentado | No documentado | Sí (~10 días) | Banca/crédito en .NET. |
| **JPauloSilvaDev/CreditSystem** | Sí | No (gestión inadimplencia) | No | — | Crédito en .NET; otro tipo de proceso. |

---

## 5. Referencia local (cumplen JD y documentación Censys)

- **Vuestro proyecto:** `credit-system` (este repo): 5 pasos, eventos (MassTransit/RabbitMQ), Worker, estados, reejecución, JWT, Docker.  
- **Candidato Censys:** `Censys-Credit-System` (orquestador in-process, Channel, 4 pasos, MediatR).  
- **Ejemplo Censys:** `C:\Users\juana\Documents\JUAN\Personal\Censys\ejemplo-challenge`: eventos (SolicitudCreada, ElegibilidadValidada, RiesgoEvaluado, CondicionesCalculadas, DecisionEmitida), RabbitMQ, API + Worker, mismo proceso que la JD.

En GitHub **no** aparece (en las búsquedas realizadas) ningún repo público en C# que replique exactamente el enunciado de la entrevista (5 pasos + eventos + background + estados). Los más cercanos son **mrfiliputti/credit-eligibility-durable-functions** (C#, workflow async, estado) y **marcin-sieminski/CreditApplications** (C#, workflow crédito entidades financieras). El resto son referentes parciales (un paso, dominio crédito, o mismo proceso en otro lenguaje).

Si quieres, en un siguiente paso se puede hacer una comparación corta “nosotros vs mrfiliputti/credit-eligibility-durable-functions” o “nosotros vs marcin-sieminski/CreditApplications” (diseño, pasos, eventos, estados).
