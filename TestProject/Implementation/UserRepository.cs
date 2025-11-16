using Work.Database;
using Work.Interfaces;

namespace Work.Implementation
{
    public class UserRepository(MockDatabase context) : IRepository<User, Guid>
    {
        private readonly MockDatabase _context = context;

        public void Create(User user)
        {
            _context.Users.Add(user.UserId, user);
        }

        public User Read(Guid key)
        {
            return _context.Users.GetValueOrDefault(key);
        }

        public void Update(User user)
        {
            if (_context.Users.TryGetValue(user.UserId, out _))
            {
                _context.Users[user.UserId] = user;
            }
            else
            {
                throw new InvalidOperationException($"Cannot update user with id {user.UserId}. User does not exist.");
            }

            //Alternative implementation if user should be added if not existing:
            //_context.Users[user.UserId] = user;
        }

        public void Remove(User user)
        {
            var userRemoved = _context.Users.Remove(user.UserId);
            if (!userRemoved)
            {
                throw new InvalidOperationException($"User removal failed for user id {user.UserId}.");
            }
        }
    }
}
