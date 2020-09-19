using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<CharacterSkill> CharacterSkills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CharacterSkill>()
            .HasKey(cs => new { cs.CharacterId, cs.SkillId });

            modelBuilder.Entity<User>()
            .Property(c => c.Role).HasDefaultValue("Player");

            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "Fireball", Damage = 30 },
                new Skill { Id = 2, Name = "Frenzy", Damage = 20 },
                new Skill { Id = 3, Name = "Blizzard", Damage = 50 }
            );

            Utility.CreatePasswordHash("password1", out byte[] passwordHash1, out byte[] passwordSalt1);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, PasswordHash = passwordHash1, PasswordSalt = passwordSalt1, Username = "user1" }
            );

            Utility.CreatePasswordHash("password2", out byte[] passwordHash2, out byte[] passwordSalt2);

            modelBuilder.Entity<User>().HasData(
               new User { Id = 2, PasswordHash = passwordHash2, PasswordSalt = passwordSalt2, Username = "user2" }
           );

            modelBuilder.Entity<Character>().HasData(
                new Character
                {
                    Id = 1,
                    Name = "Frodo",
                    Class = RpgClass.Knight,
                    HitPoints = 100,
                    Strength = 15,
                    Defence = 10,
                    Intelligence = 10,
                    UserId = 1
                },
                new Character
                {
                    Id = 2,
                    Name = "Raistlin",
                    Class = RpgClass.Mage,
                    HitPoints = 100,
                    Strength = 5,
                    Defence = 5,
                    Intelligence = 20,
                    UserId = 2
                }
            );

            modelBuilder.Entity<Weapon>().HasData(
                new Weapon
                {
                    Id = 1,
                    Name = "The Master Sword",
                    Damage = 20,
                    CharacterId = 1
                },
                new Weapon
                {
                    Id = 2,
                    Name = "Crystal Wand",
                    Damage = 5,
                    CharacterId = 2
                }
            );

            modelBuilder.Entity<CharacterSkill>().HasData(
                new CharacterSkill
                {
                    CharacterId = 1,
                    SkillId = 2
                },
                new CharacterSkill
                {
                    CharacterId = 2,
                    SkillId = 1
                },
                new CharacterSkill
                {
                    CharacterId = 2,
                    SkillId = 3
                }
            );
        }
    }
}