using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameStoreData
{
    private readonly List<Genre> genres =
[
    new Genre {Id = new Guid("9b135791-0313-4d78-a335-eb86b60a59d9"), Name = "Fighting"},
    new Genre {Id = new Guid("0ca91115-8bb0-445b-bd6d-13732d36b9f9"), Name = "Kids and Family"},
    new Genre {Id = new Guid("e619d688-209a-48f1-ae02-ded3d0efb80f"), Name = "Racing"},
    new Genre {Id = new Guid("893aecbf-8df6-4088-bfab-0332f0052067"), Name = "Roleplaying"},
    new Genre {Id = new Guid("f22f896f-c399-4b66-ad69-04cfed1a2c87"), Name = "Sports"}
];

    private readonly List<Game> games;

    public GameStoreData()
    {
        games =
        [
            new Game {
                Id = Guid.NewGuid(),
                Name = "Street Fighter II",
                Genre = genres[0],
                Price = 19.99m,
                ReleaseDate = new DateOnly(1992, 7, 5),
                Description = "Test description"
            },
            new Game {
                Id = Guid.NewGuid(),
                Name = "Final Fantasy XIV",
                Genre = genres[3],
                Price = 59.99m,
                ReleaseDate = new DateOnly(2010, 9, 30),
                Description = "Description 2"
            },
            new Game {
                Id = Guid.NewGuid(),
                Name = "FIFA 23",
                Genre = genres[4],
                Price = 69.99m,
                ReleaseDate = new DateOnly(2022, 9, 27),
                Description = "description 3"
            }
        ];
    }

    public IEnumerable<Game> GetGames() => games;

    public Game? GetGame(Guid id) => games.Find(g => g.Id == id);

    public void AddGame(Game game)
    {
        game.Id = Guid.NewGuid();
        games.Add(game);
    }

    public void RemoveGame(Guid id)
    {
        games.RemoveAll(g => g.Id == id);
    }

    public IEnumerable<Genre> GetGenres() => genres;

    public Genre? GetGenre(Guid id) => genres.Find(g => g.Id == id);
}
