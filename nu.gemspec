# -*- encoding: utf-8 -*-

Gem::Specification.new do |s|
  s.name        = "nu"
  s.version     = "0.1.4"
  s.platform    = Gem::Platform::RUBY
  s.authors     = ["Dru Sellers","Chris Patterson", "Rob Reynold"]
  s.email       = ["nu-net@googlegroups.com"]
  s.homepage    = "http://groups.google.com/group/nu-net"
  s.summary     = "The best way to manage your application's dependencies in .net"
  s.description = "Nu manages an application's dependencies through its entire life, across many machines, systematically and repeatably"

  s.required_rubygems_version = ">= 1.3.6"
  s.rubyforge_project         = "nu"

  s.files        = Dir.glob("{bin,lib}/**/*")
  s.executables  = ['nu']
  s.bindir = 'bin'
  s.require_path = 'lib'
end