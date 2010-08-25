# -*- encoding: utf-8 -*-
lib = File.expand_path('../lib/', __FILE__)
$:.unshift lib unless $:.include?(lib)

Gem::Specification.new do |s|
  s.name        = "nu_test_has_different_versions"
  s.version     = "0.2.1"
  s.platform    = Gem::Platform::RUBY
  s.summary     = "a test gem for nu specs"
	s.files        = Dir.glob("{lib}/**/*")
end