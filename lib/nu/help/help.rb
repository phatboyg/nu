require File.expand_path(File.dirname(__FILE__) + "/../gem_tools.rb")

module Help

	def help_doc_path
		gtool = Nu::GemTools.new
		File.expand_path(File.join(gtool.spec_for('nu').full_gem_path,'docs'))
	end
	
	def help_doc_for(descriptor)
		log "help_doc_for called: descriptor: #{descriptor.inspect}"
		
		if descriptor.is_a?(String)
			descriptor.downcase!
			descriptor_description = descriptor.gsub(/-/, ": ")
		end
		
		if descriptor.is_a?(Array)
			descriptor.map!{|item| item.downcase}
			descriptor_description = descriptor.join(': ')
			descriptor = descriptor.join('-')
		end
		
		file_path = File.join(help_doc_path, descriptor + ".md")
		log "Looking for file at: #{file_path}"
		
		return "No help found for \"#{descriptor_description}\"" unless File.exists?(file_path)
		
		filter_markdown(IO.read(file_path))
	end
	
	def filter_markdown(from)
		#links
		from.gsub!(/\[\[(.*)\|.*\]\]/,'\1')
		from.gsub!(/\[\[(.*)\]\]/,'\1')
		
		return from
	end
	
end