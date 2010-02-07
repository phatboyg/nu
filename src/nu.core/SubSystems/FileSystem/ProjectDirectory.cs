// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace nu.core.SubSystems.FileSystem
{
    using System;
    using NDepend.Helpers.FileDirectoryPath;

    /// <summary>
    /// where the '.nu' folder is
    /// </summary>
    public class ProjectDirectory
    {
        readonly DirectoryPathAbsolute _libDirectory;
        readonly DirectoryPathAbsolute _projectDirectory;

        public ProjectDirectory(WorkingDirectory workingDirectory)
        {
            _projectDirectory = workingDirectory.WorkWithMe(WalkThePathLookingForNu);
            if (_projectDirectory != null)
                _libDirectory = _projectDirectory.GetBrotherDirectoryWithName("lib");
            _libDirectory.Create();
        }

        public string Path
        {
            get { return _projectDirectory.Path; }
        }

        public void Install(object info)
        {
            if (!FoundAProject())
                throw new Exception("not in a nu project path");

            var targetDir = _libDirectory.GetChildDirectoryWithName("info.Name");
            if (!targetDir.Exists)
            {
                targetDir.Create();
            }

            //info.Path.CopyTo(_libDirectory.GetChildDirectoryWithName(info.Name));
        }

        public bool FoundAProject()
        {
            return _projectDirectory != null;
        }

        public DirectoryPathAbsolute WalkThePathLookingForNu(DirectoryPathAbsolute path)
        {
            DirectoryPathAbsolute result = null;

            if (!path.IsRoot())
            {
                DirectoryPathAbsolute bro = path.GetBrotherDirectoryWithName(".nu");
                if (bro.Exists)
                {
                    return bro;
                }
                if (path.HasParentDir)
                {
                    result = WalkThePathLookingForNu(path.ParentDirectoryPath);
                }
            }


            return result;
        }
    }
}