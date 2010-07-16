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
	  gems = Gem.source_index.find_name name
	  return gems.last if gems.length > 0
    end
    
    def available? (name)
      Gem.available? name
    end
  end
end
