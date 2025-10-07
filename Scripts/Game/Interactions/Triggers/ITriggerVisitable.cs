namespace FAS.Triggers
{
	public interface ITriggerVisitable
	{
		public void Accept(ITriggerVisitor visitor);
	}
}