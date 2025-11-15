using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository : IRepository<User, Guid>
    {
        private readonly MockDatabase _context;

        public UserRepository(MockDatabase context)
        {
            _context = context;
        }
        public void Create(User user)
        {
            _context.Users.Add(user.UserId, user);
        }

        public User Read(Guid key)
        {
            //TODO: Decide on null reference handling
            return _context.Users.GetValueOrDefault(key);
        }

        public void Update(User user)
        {
            if (_context.Users.TryGetValue(user.UserId, out _))
            {
                _context.Users[user.UserId] = user;
            }

            //Alternative implementation if user should be added if not existing:
            //_context.Users[user.UserId] = user;
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user.UserId);
        }
    }
}
