require 'rubygems'
require 'ostruct'

module Nu
	class SettingsExtension
		
		def self.mix_in(target)

			def target.set_setting_by_path(path, value, logger)
				path = path.split('.') if path.class == String
				set_setting(self, path, value, logger)
			end
		
			def target.get_setting_by_path(path, logger)
				path = path.split('.') if path.class == String
				obj = self
				path.length.times do 
					if path.length == 1
						return obj.send(path.to_s)
					else
						part = path.shift
						obj = obj.send(part.to_s)
						return nil if obj == nil
					end
				end
				send(name)
			end
		
			def target.set_setting(settings_object, path, value, logger)
				if path.length == 1
					logger.call("Assigning value: #{value.to_s}")
					begin
						if value.match(/^\d*\.\d*$/)
							value = Float(value.to_s)
						else
							value = Integer(value.to_s)
						end
					rescue
						logger.call("Could not coerce into a number.")
						value = true if value.to_s.match(/(^true$|^t$|^yes$|^y$)/i) != nil
						value = false if value.to_s.match(/(^false$|^f$|^no$|^n$)/i) != nil
					end

					settings_object.send(path.to_s + "=", value) 
				else
					prop = path.shift
					settings_object.send(prop + "=", OpenStruct.new) if settings_object.send(prop) == nil
					set_setting(settings_object.send(prop), path, value, logger)
				end
			end
			
		end
	
	end
end