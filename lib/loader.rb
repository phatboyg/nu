require 'rubygems'

module Nu
  class Loader
    
    def get_libdir(name)
      g = get_gemspec name
      l = Gem.searcher.lib_dirs_for g
      #scrub and return?
      l
    end
    
    def get_gemspec(name)
      Gem.searcher.find name
    end
    
    def available? (name)
      Gem.available? name
    end
  end
end
