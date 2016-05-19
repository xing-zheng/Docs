Introduction to ASP.NET Core
============================

By `Daniel Roth`_ and `Rick Anderson`_

ASP.NET Core is a significant redesign of ASP.NET. This topic introduces the new concepts in ASP.NET Core and explains how they help you develop modern web apps.

.. contents:: Sections:
  :local:
  :depth: 1

What is ASP.NET Core?
---------------------

ASP.NET Core is a new open-source and cross-platform framework for building modern cloud-based Web apps using .NET. We built it from the ground up to provide an optimized development framework for apps that are deployed to the cloud or run on-premises. It consists of modular components with minimal overhead, so you retain flexibility while constructing your solutions. You can develop and run your ASP.NET Core apps cross-platform on Windows, Mac and Linux. ASP.NET Core is open source at `GitHub <https://github.com/aspnet/home>`_.

Why build ASP.NET Core?
-----------------------

The first preview release of ASP.NET came out almost 15 years ago as part of the .NET Framework.  Since then millions of developers have used it to build and run great web apps, and over the years we have added and evolved many capabilities to it.

ASP.NET Core has a number of architectural changes that result in a much leaner and modular framework. ASP.NET Core is no longer based on *System.Web.dll*, but is instead based on a set of granular and well factored `NuGet <http://www.nuget.org/>`__ packages allowing you to optimize your app to include just the NuGet packages you need. You can reduce your apps surface area to improve security, reduce  servicing and improve performance in a pay-for-what-you-use model.

ASP.NET Core is built with the needs of modern Web apps in mind, including a unified story for building Web UI and Web APIs that integrate with today's modern client-side frameworks and development workflows. ASP.NET Core is built to be cloud-ready by introducing environment-based configuration and by providing built-in :doc:`dependency injection </fundamentals/dependency-injection>` support.

To appeal to a broader audience of developers, ASP.NET Core supports cross-platform development on Windows, Mac and Linux. The entire ASP.NET Core stack is open source and encourages community contributions and engagement. ASP.NET Core comes with a new, agile project system in Visual Studio while also providing a complete command-line interface so that you can develop using the tools of your choice. `Visual Studio Code <https://code.visualstudio.com/#alt-downloads>`__ runs on a variety of platforms.

In summary, with ASP.NET Core you gain the following foundational improvements:

- New light-weight and modular HTTP request pipeline
- Ability to host on IIS or self-host in your own process
- Built on `.NET Core`_, which supports true side-by-side app versioning
- Ships entirely as `NuGet <http://www.nuget.org/>`__  packages
- Integrated support for `creating and using NuGet packages <https://docs.nuget.org/create/creating-and-publishing-a-package>`__
- Single aligned web stack for Web UI and Web APIs
- Cloud-ready environment-based configuration
- Built-in support for dependency injection
- New tooling that simplifies modern web development
- Build and run cross-platform ASP.NET apps on Windows, Mac and Linux
- Open source and community focused

Application anatomy
-------------------

.. comment In RC1, The work of the WebHostBuilder was hidden in dnx.exe

An ASP.NET Core app self-hosts in a console app using ``IWebHost``. ``Main`` creates and runs the web server using `WebHostBuilder <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Hosting/WebHostBuilder/index.html>`__ :

.. literalinclude:: /getting-started/sample/aspnetcoreapp/Program.cs
    :language: c#

- It's simply a console app with a ``Main`` method
- `WebHostBuilder <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Hosting/WebHostBuilder/index.html>`__ is a builder (it follows the build pattern) which creates a hosting engine for ``IWebHost`` and builds the HTTP pipeline
- `WebHostBuilder <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Hosting/WebHostBuilder/index.html>`__ has methods to define ``startup`` and web server type

  - In this sample the `Kestrel <https://docs.asp.net/en/dev/fundamentals/servers.html#kestrel>`__ web server is used, but other :doc:`web servers </fundamentals/servers>` can be specified
  - `UseStartup <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Hosting/WebHostBuilder/index.html>`__ specifies the ``Startup`` method, we'll show that in the next section
  - Provides other methods such as ``UseIISIntegration`` (for hosting in IIS and IIS Express) and ``UseContentRoot`` (which specifies the content root directory) and more
- ``Build`` builds the ``IWebHost`` which hosts the web app. 
- ``Run`` runs the web host and starts listening for requests

Startup
---------------------------

The ``Startup`` class (``UseStartup<Startup>`` in the code above) must be public and contain the following methods:

.. code-block:: c#

  public class Startup
  {
      public void ConfigureServices(IServiceCollection services)
      {
      }

      public void Configure(IApplicationBuilder app)
      {
      }
  }

- :doc:`ConfigureServices </fundamentals/startup>`  defines the services used by your app (such as the ASP.NET MVC Core framework, Entity Framework Core, Identity, etc. )
- :doc:`Configure </fundamentals/startup>` defines the :doc:`middleware </fundamentals/middleware>` in the request pipeline
- See :doc:`/fundamentals/startup`

Services
--------

A service is a component that is intended for common consumption in an application. Services are made available through dependency injection. ASP.NET Core includes a simple built-in inversion of control (IoC) container that supports constructor injection by default, but can be easily replaced with your IoC container of choice. In addition to the loose coupling benefit DI provides, it also makes services available throughout your app. For example, :doc:`Logging </fundamentals/logging>` is available throughout your app. See :doc:`/fundamentals/dependency-injection` for more details.

Services in ASP.NET Core come in three varieties: singleton, scoped and transient. Transient services are created each time they’re requested from the container. Scoped services are created only if they don’t already exist in the current scope. For Web applications, a container scope is created for each request, so you can think of scoped services as per request. Singleton services are only created once. See `Service Lifetimes and Registration Options <https://docs.asp.net/en/latest/fundamentals/dependency-injection.html#service-lifetimes-and-registration-options>`__ for more information.

Middleware
----------

In ASP.NET Core you compose your request pipeline using :doc:`/fundamentals/middleware`. ASP.NET Core middleware performs asynchronous logic on an ``HttpContext`` and then optionally  invokes the next middleware in the sequence or terminates the request directly. You generally "Use" middleware by invoking a corresponding ``UseXYZ`` extension method on the ``IApplicationBuilder`` in the ``Configure`` method.

ASP.NET Core comes with a rich set of prebuilt middleware:

- :doc:`/fundamentals/static-files`
- :doc:`/fundamentals/routing`
- :doc:`/fundamentals/diagnostics`
- :doc:`/security/authentication/index`

You can also author your own :doc:`custom middleware </fundamentals/middleware>`.

You can use any `OWIN <http://owin.org>`_-based middleware with ASP.NET Core. See :doc:`/fundamentals/owin` for details.

Servers
-------

The ASP.NET Core hosting model does not directly listen for requests, but instead relies on an HTTP :doc:`server </fundamentals/servers>` implementation to surface the request to the application as a set of feature interfaces that can be composed into an ``HttpContext``. ASP.NET Core includes a managed cross-platform web server, called :ref:`Kestrel <kestrel>`, that you would typically run behind a production web server like `IIS <https://iis.net>`__ or `nginx <http://nginx.org>`__.

Web root
--------

The web root of your app is the root location in your project from which HTTP requests are handled (for example handling of static file requests). The web root of an ASP.NET Core app is configured using the ``webroot`` property in the *project.json* file.

Configuration
-------------

ASP.NET Core uses a new configuration model for handling of simple name-value pairs that is not based on ``System.Configuration`` or *web.config*. This new configuration model pulls from an ordered set of configuration providers. The built-in configuration providers support a variety of file formats (XML, JSON, INI) and environment variables to enable environment-based configuration. You can also write your own custom configuration providers. Environments, like "Development"" and "Production", are a first-class notion in ASP.NET Core and can also be set using environment variables:

.. literalinclude:: /../common/samples/WebApplication1/src/WebApplication1/Startup.cs
  :language: c#
  :lines: 22-34
  :dedent: 12

See :doc:`/fundamentals/configuration` for more details on the new configuration system and :doc:`/fundamentals/environments` for details on how to work with environments in ASP.NET Core.

Build Web UI and Web APIs using MVC
-----------------------------------

- ASP.NET Core helps you create well-factored and testable web apps. See :doc:`/mvc/index` and :doc:`/testing/index`.
- `Razor <http://www.asp.net/web-pages/overview/getting-started/introducing-razor-syntax-c>`__ provides a productive language to create :doc:`Views </mvc/views/index>`
- :doc:`Tag Helpers </mvc/views/tag-helpers/intro>` enable server-side code to participate in creating and rendering HTML elements in Razor files. Tag Helpers are one of the most popular new features of ASP.NET Core Mvc
- You can create HTTP services with full support for content negotiation using custom or built-in formatters (JSON, XML)

Client-side development
-----------------------

ASP.NET Core is designed to integrate seamlessly with a variety of client-side frameworks, including :doc:`AngularJS </client-side/angular>`, :doc:`KnockoutJS </client-side/knockout>` and :doc:`Bootstrap </client-side/bootstrap>`. See :doc:`/client-side/index` for more details.

Next steps
----------

- :doc:`/tutorials/first-mvc-app/index`
- :doc:`/tutorials/your-first-mac-aspnet`
- :doc:`/tutorials/first-web-api`
- :doc:`/fundamentals/index`