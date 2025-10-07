namespace FAS
{
	public interface IInteractableVisitable
	{
		public void Accept(IInteractableVisitor visitor);
	}
}