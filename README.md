HappyPath
=========

WebApi/MVC/EF Happy Path Project


Get Started
=========

1. Download the project
2. Run `Update-Database` in the package manager console.
3. Build


Start from Scratch
=========

#### To start a new project - New Project -> MVC + Web Api Project

```
uninstall-package bootstrap
uninstall-package jQuery

install-package EntityFramework
Install-Package Microsoft.AspNet.WebPages
install-package Microsoft.AspNet.WebApi
install-package Microsoft.AspNet.Mvc
install-package Microsoft.AspNet.Web.Optimization
install-package Ninject
install-package Ninject.Extensions.Conventions
install-package Ninject.MVC5
install-package Ninject.Web.WebApi
```


#### Right-Click Solution -> Add New Project -> HappyPath.Services

```
install-package EntityFramework
install-package Ninject
install-package Ninject.Extensions.Conventions

Enable-Migrations
Add-Migration Initial
Update-Database

F5 :)
```