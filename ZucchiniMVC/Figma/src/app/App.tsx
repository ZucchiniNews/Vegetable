import { Newspaper, Clock, TrendingUp, Calendar } from 'lucide-react';

const newsArticles = [
  {
    id: 1,
    title: "Global Climate Summit Reaches Historic Agreement on Carbon Emissions",
    excerpt: "World leaders unite to commit to unprecedented climate action with new binding targets for 2030.",
    category: "Environment",
    image: "https://images.unsplash.com/photo-1569163139394-de4798aa62b6?w=800&q=80",
    date: "March 23, 2026",
    readTime: "5 min read",
    featured: true
  },
  {
    id: 2,
    title: "Tech Giants Announce Breakthrough in Quantum Computing",
    excerpt: "Major advancement brings practical quantum computers closer to reality, promising to revolutionize industries.",
    category: "Technology",
    image: "https://images.unsplash.com/photo-1635070041078-e363dbe005cb?w=800&q=80",
    date: "March 22, 2026",
    readTime: "4 min read"
  },
  {
    id: 3,
    title: "Stock Markets Rally on Positive Economic Indicators",
    excerpt: "Major indices reach new highs as unemployment drops and consumer confidence soars.",
    category: "Business",
    image: "https://images.unsplash.com/photo-1611974789855-9c2a0a7236a3?w=800&q=80",
    date: "March 22, 2026",
    readTime: "3 min read"
  },
  {
    id: 4,
    title: "Revolutionary Medical Treatment Shows Promise Against Rare Disease",
    excerpt: "Clinical trials demonstrate effectiveness of new gene therapy approach with minimal side effects.",
    category: "Health",
    image: "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?w=800&q=80",
    date: "March 21, 2026",
    readTime: "6 min read"
  },
  {
    id: 5,
    title: "Space Agency Confirms Plans for Lunar Base by 2030",
    excerpt: "Ambitious project aims to establish permanent human presence on the Moon within four years.",
    category: "Science",
    image: "https://images.unsplash.com/photo-1446776653964-20c1d3a81b06?w=800&q=80",
    date: "March 21, 2026",
    readTime: "5 min read"
  },
  {
    id: 6,
    title: "Renewable Energy Surpasses Fossil Fuels in Power Generation",
    excerpt: "Historic milestone achieved as solar and wind energy become primary sources of electricity.",
    category: "Environment",
    image: "https://images.unsplash.com/photo-1509391366360-2e959784a276?w=800&q=80",
    date: "March 20, 2026",
    readTime: "4 min read"
  }
];

const categories = ["All", "Environment", "Technology", "Business", "Health", "Science"];

export default function App() {
  return (
    <div className="size-full bg-white overflow-auto">
      {/* Header */}
      <header className="border-b sticky top-0 bg-white z-10">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2">
              <Newspaper className="w-8 h-8" />
              <h1 className="text-2xl font-bold">Daily News</h1>
            </div>
            <nav className="hidden md:flex gap-6">
              <a href="#" className="hover:text-gray-600 transition-colors">Home</a>
              <a href="#" className="hover:text-gray-600 transition-colors">World</a>
              <a href="#" className="hover:text-gray-600 transition-colors">Politics</a>
              <a href="#" className="hover:text-gray-600 transition-colors">Business</a>
              <a href="#" className="hover:text-gray-600 transition-colors">Tech</a>
            </nav>
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        {/* Category Filter */}
        <div className="flex gap-3 mb-8 overflow-x-auto pb-2">
          {categories.map((category) => (
            <button
              key={category}
              className={`px-4 py-2 rounded-full whitespace-nowrap transition-colors ${
                category === "All"
                  ? "bg-black text-white"
                  : "bg-gray-100 hover:bg-gray-200"
              }`}
            >
              {category}
            </button>
          ))}
        </div>

        {/* Featured Article */}
        {newsArticles
          .filter((article) => article.featured)
          .map((article) => (
            <article
              key={article.id}
              className="mb-12 rounded-2xl overflow-hidden bg-gray-50 hover:shadow-lg transition-shadow cursor-pointer"
            >
              <div className="grid md:grid-cols-2 gap-6">
                <div className="relative h-64 md:h-auto">
                  <img
                    src={article.image}
                    alt={article.title}
                    className="w-full h-full object-cover"
                  />
                  <div className="absolute top-4 left-4">
                    <span className="bg-red-500 text-white px-3 py-1 rounded-full text-sm">
                      Featured
                    </span>
                  </div>
                </div>
                <div className="p-6 md:p-8 flex flex-col justify-center">
                  <div className="flex items-center gap-3 mb-3">
                    <span className="px-3 py-1 bg-black text-white rounded-full text-sm">
                      {article.category}
                    </span>
                    <TrendingUp className="w-4 h-4 text-red-500" />
                  </div>
                  <h2 className="text-3xl md:text-4xl font-bold mb-4 leading-tight">
                    {article.title}
                  </h2>
                  <p className="text-gray-600 text-lg mb-6">{article.excerpt}</p>
                  <div className="flex items-center gap-4 text-gray-500 text-sm">
                    <div className="flex items-center gap-1">
                      <Calendar className="w-4 h-4" />
                      <span>{article.date}</span>
                    </div>
                    <div className="flex items-center gap-1">
                      <Clock className="w-4 h-4" />
                      <span>{article.readTime}</span>
                    </div>
                  </div>
                </div>
              </div>
            </article>
          ))}

        {/* Latest News Grid */}
        <div className="mb-6">
          <h2 className="text-2xl font-bold mb-6">Latest News</h2>
          <div className="grid sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {newsArticles
              .filter((article) => !article.featured)
              .map((article) => (
                <article
                  key={article.id}
                  className="rounded-xl overflow-hidden bg-white border hover:shadow-lg transition-shadow cursor-pointer"
                >
                  <div className="relative h-48">
                    <img
                      src={article.image}
                      alt={article.title}
                      className="w-full h-full object-cover"
                    />
                  </div>
                  <div className="p-5">
                    <div className="mb-3">
                      <span className="px-3 py-1 bg-gray-100 rounded-full text-sm">
                        {article.category}
                      </span>
                    </div>
                    <h3 className="font-bold text-lg mb-2 leading-tight">
                      {article.title}
                    </h3>
                    <p className="text-gray-600 text-sm mb-4">{article.excerpt}</p>
                    <div className="flex items-center gap-3 text-gray-500 text-xs">
                      <div className="flex items-center gap-1">
                        <Calendar className="w-3 h-3" />
                        <span>{article.date}</span>
                      </div>
                      <div className="flex items-center gap-1">
                        <Clock className="w-3 h-3" />
                        <span>{article.readTime}</span>
                      </div>
                    </div>
                  </div>
                </article>
              ))}
          </div>
        </div>
      </main>

      {/* Footer */}
      <footer className="border-t mt-12 bg-gray-50">
        <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
          <div className="text-center text-gray-600 text-sm">
            <p>© 2026 Daily News. All rights reserved.</p>
          </div>
        </div>
      </footer>
    </div>
  );
}