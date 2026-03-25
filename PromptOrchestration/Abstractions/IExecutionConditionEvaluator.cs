namespace PromptOrchestration;

public interface IExecutionConditionEvaluator
{
	bool shouldRun(ConditionDefinition conditionDefinition, WorkflowRunContext workflowRunContext);
}
