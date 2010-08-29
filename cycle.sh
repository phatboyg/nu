#!/bin/sh

rm *.gem

gem uninstall nu -a -x

gem build nu.gemspec

gem install --no-rdoc --no-ri nu
