# -*- encoding: utf-8 -*-
lib = File.expand_path('../lib/', __FILE__)
$:.unshift lib unless $:.include?(lib)

Gem::Specification.new do |s|
  s.name        = "nu_test_has_require_paths"
  s.version     = "0.0.1"
  s.platform    = Gem::Platform::RUBY
  s.summary     = "a test gem for nu specs"

  s.require_path = 'not_lib'
	s.files        = Dir.glob("{not_lib}/**/*")
end
