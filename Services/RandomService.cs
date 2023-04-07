namespace ProvaPub.Services
{
	public class RandomService
	{
        private readonly Random random;
        public RandomService()
		{
			random = new Random(Guid.NewGuid().GetHashCode());
		}
		public int GetRandom()
		{
            return random.Next(100);
        }

	}
}
