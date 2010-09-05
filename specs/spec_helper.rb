require 'rubygems'
require 'spec'
require 'mocha'
require 'ostruct'

Spec::Runner.configure do |config|
  config.mock_with :mocha
end