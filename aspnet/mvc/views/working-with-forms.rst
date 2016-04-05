Working with Forms
============================

By `Rick Anderson`_, `Dave Paquette <https://twitter.com/Dave_Paquette>`_ and `Jerrie Pelser <https://twitter.com/jerriepelser>`__

This document demonstrates working with Forms and the HTML elements commonly used on a Form. The HTML `Form <https://www.w3.org/TR/html401/interact/forms.html>`__ element provides the primary mechanism web apps use to postback data to the server. Most of this document describes :doc:`Tag Helpers <tag-helpers/intro>` and how they can help you productively create robust HTML forms. We recommend you read :doc:`tag-helpers/intro` before you read this document.

In many cases, :doc:`HTML Helpers </mvc/views/html-helpers>` provide an alternative approach to a specific Tag Helper, but it's important to recognize that Tag Helpers do not replace HTML Helpers and there is not a Tag Helper for each HTML Helper. When an HTML Helper alternative exists, it is mentioned.


.. contents:: Sections:
  :local:
  :depth: 1

The Form Tag Helper
---------------------
  
The `Form <https://www.w3.org/TR/html401/interact/forms.html>`__ Tag Helper:

- Generates the HTML `<FORM> <https://www.w3.org/TR/html401/interact/forms.html>`__ ``action`` attribute value for a MVC controller action or named route
- Generates a hidden `Request Verification Token <http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages>`__ to prevent cross-site request forgery
- Supports the Tag Helper attribute ``asp-route-returnurl`` which provides a return URL
- HTML Helper alternative: ``Html.BeginForm``

Sample:

.. literalinclude::   forms/sample/final/Views/Demo/RegisterFormOnly.cshtml
  :language: HTML

The Form Tag Helper above generates the following HTML:
 
.. code-block:: HTML

  <form method="post" action="/Demo/Register">
    <!-- Input and Submit elements -->
    <input name="__RequestVerificationToken" type="hidden" value="<removed for brevity>" />
   </form>
  
The MVC runtime generates the ``action`` attribute value from the Form Tag Helper attributes ``asp-controller`` and ``asp-action``. The Form Tag Helper also generates a hidden `Request Verification Token <http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages>`__ to prevent cross-site request forgery. Protecting a pure HTML Form from cross-site request forgery is very difficult, the Form Tag Helper provides this service for you.

Many of the views in the  *Views/Account* folder (generated when you create a new web app with *Individual User Accounts*)contain the ``asp-route-returnurl`` attribute:

.. code-block:: HTML
  :emphasize-lines: 2
  
  <form asp-controller="Account" asp-action="Login" 
    asp-route-returnurl="@ViewData["ReturnUrl"]" 
    method="post" class="form-horizontal" role="form">

:Note: With the built in templates, ``returnUrl`` is only populated automatically when you try to access an authorized resource but are not authenticated or authorized. When you attempt an unauthorized access, the security middleware redirects you to the login page with the ``returnUrl`` set.

Using a named route
^^^^^^^^^^^^^^^^^^^

The ``asp-route`` Tag Helper attribute can also generate markup for the HTML ``action`` attribute. An app with a :doc:`route </fundamentals/routing>`  named ``register`` could use the following markup for the registration page:
 
.. literalinclude::  forms/sample/final/Views/Demo/RegisterRoute.cshtml 
  :language: HTML
  :emphasize-lines: 4

The Input Tag Helper
---------------------
 
The Input Tag Helper binds an HTML `<input> <https://www.w3.org/wiki/HTML/Elements/input>`__ element to a model property in your razor view.

The Input Tag Helper:

- Generates the ``id`` and ``name`` HTML attributes for the model name specified in the ``asp-for`` attribute.
- Sets the HTML ``type`` attribute value based on the model type and  `data annotation <https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx>`__ attributes applied to the model property
- Generates `HTML5 <https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/HTML5>`__  validation attributes from `data annotation <https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx>`__ attributes applied to model properties
- Will not overwrite the HTML ``type`` attribute value when one is specified 
- HTML Helper alternative: ``Html.EditorFor``
 
The ``Input`` Tag Helper sets the HTML ``type`` attribute based on the .Net type: 

+---------------------+--------------------+
|.NET type            |  Input Type        |  
+=====================+====================+
|Bool                 |  type="checkbox"   |
+---------------------+--------------------+  
|String               |  type="text"       |
+---------------------+--------------------+  
|DateTime             |  type="datetime"   |
+---------------------+--------------------+  
|Byte                 |  type="number"     |
+---------------------+--------------------+  
|Int                  |  type="number"     |
+---------------------+--------------------+  
|Single, Double       |  type="number"     |
+---------------------+--------------------+  

The following table shows `data annotations <https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx>`__ attributes that the input tag helper will map to specific input types:

+-------------------------------+--------------------+
|Attribute                      |  Input Type        |  
+===============================+====================+
|[EmailAddress]                 |  type="email"      |
+-------------------------------+--------------------+  
|[Url]                          |  type="url"        |
+-------------------------------+--------------------+  
|[HiddenInput]                  |  type="hidden"     |
+-------------------------------+--------------------+  
|Phone]                         |  type="tel"        |
+-------------------------------+--------------------+   
|[DataType(DataType.Password)]  |  type="password"   |
+-------------------------------+--------------------+  
|[DataType(DataType.Date)]      |  type="date"       |
+-------------------------------+--------------------+  
|[DataType(DataType.Time)]      |  type="time"       |
+-------------------------------+--------------------+  
 
Sample: 
 
.. literalinclude::  forms/sample/final/ViewModels/RegisterViewModel.cs
  :language: c#

.. literalinclude::  forms/sample/final/Views/Demo/RegisterInput.cshtml
  :language: HTML

The code above generates the following HTML:

.. code-block:: HTML

    <form method="post" action="/Demo/RegisterInput">
      Email:  
      <input type="email" data-val="true" 
             data-val-email="The Email Address field is not a valid e-mail address." 
             data-val-required="The Email Address field is required." 
             id="Email" name="Email" value="" /> <br>
      Password: 
      <input type="password" data-val="true" 
             data-val-required="The Password field is required." 
             id="Password" name="Password" /><br>
      <button type="submit">Register</button>
    <input name="__RequestVerificationToken" type="hidden" value="<removed for brevity>" />
  </form>

The data annotations applied to the ``Email`` and ``Password`` properties generated meta data on the model in the form of `HTML5 <https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/HTML5>`__ ``data-val-*`` attributes (see :doc:`/mvc/models/validation`). (``data-val-`` specifies data validation.) The Input Tag Helper consumes the model metadata and produces HTML5-compatible attributes describing the validators to attach to the input fields. This provides unobtrusive HTML5 and `jQuery <https://jquery.com/>`__ validation. The unobtrusive attributes have the format ``data-val-rule="Error Message"``, where rule is the validation rule (such as required, email format, minimum string length, and maximum string length). If an error message is provided in the attribute, it is displayed as the value for the ``data-val-rule`` attribute. 

Navigating child properties
^^^^^^^^^^^^^^^^^^^^^^^^^^^^

You can also navigate to child properties of your view model. Consider a more complex model class that contains a child ``Address`` property.

.. literalinclude::  forms/sample/final/ViewModels/AddressViewModel.cs
  :language: c#
  :lines: 5-16
  :dedent: 3
  :emphasize-lines: 1-4,11

In the view, we bind to ``Address.AddressLine1``: 

.. literalinclude::  forms/sample/final/Views/Demo/RegisterAddress.cshtml 
  :language: HTML
  :emphasize-lines: 6

The following HTML is generated for `` Address.AddressLine1``:

.. code-block:: HTML

  <input type="text" id="Address_AddressLine1" name="Address.AddressLine1" value="" />
  
The Textarea Tag Helper
-------------------------

The `Textarea Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/TextAreaTagHelper/index.html>`__ tag helper is  similar to the Input Tag Helper.  

- Generates the ``id`` and ``name`` attribute, and the data validation attributes from the model for a `<textarea> <http://www.w3.org/wiki/HTML/Elements/textarea>`__ element. 
- HTML Helper alternative: ``Html.TextAreaFor``

Consider the following form:

..  literalinclude::  forms/sample/final/Views/Demo/RegisterTextArea.cshtml
  :language: HTML
  :emphasize-lines: 4

Using the following model:

.. literalinclude::  forms/sample/final/ViewModels/DescriptionViewModel.cs
  :language: c#
  
The following HTML is generated:

.. code-block:: HTML  
  :emphasize-lines: 2-8
  :linenos:

  <form method="post" action="/Demo/RegisterTextArea">
    <textarea data-val="true" 
     data-val-maxlength="The field Description must be a string or array type with a maximum length of &#x27;1024&#x27;."
     data-val-maxlength-max="1024" 
     data-val-minlength="The field Description must be a string or array type with a minimum length of &#x27;5&#x27;." 
     data-val-minlength-min="5" 
     id="Description" name="Description">
    </textarea>
    <button type="submit">Test</button>
    <input name="__RequestVerificationToken" type="hidden" value="<removed for brevity>" />
  </form>
  
The Validation Tag Helper
---------------------------

There are two Validation Tag Helpers. The `Validation Message Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ValidationMessageTagHelper/index.html>`__ (which displays a validation message for a single property on your model), and the `Validation Summary Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ValidationSummaryTagHelper/index.html>`__ (which displays a summary of validation errors). The `Input Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/InputTagHelper/index.html>`__ adds HTML5 client side validation attributes to input elements based on data annotation attributes on your model classes. The Validation Tag Helper displays these error messages when a validation error occurs. 

The `Validation Message Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ValidationMessageTagHelper/index.html>`__  is used with the ``asp-validation-for`` attribute on a HTML `span <https://developer.mozilla.org/en-US/docs/Web/HTML/Element/span>`__ element.

.. code-block:: HTML
  
  <span asp-validation-for="Email">
  
The Validation Message Tag Helper will generate the following HTML:

.. code-block:: HTML

    <span class="field-validation-valid" 
      data-valmsg-for="Email" 
      data-valmsg-replace="true">

The `Validation Message Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ValidationMessageTagHelper/index.html>`__  adds the `HTML5 <https://developer.mozilla.org/en-US/docs/Web/Guide/HTML/HTML5>`__  ``data-valmsg-for="property"`` attribute to the ``<span>`` element, which attaches the validation error messages on the input field of the specified model property with ``<span>`` element. When a client side validation error occurs, `jQuery <https://jquery.com/>`__ displays the error message in the ``<span>`` element.

You generally use the `Validation Message Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ValidationMessageTagHelper/index.html>`__  after an ``Input`` Tag Helper for the same property. Doing so displays any validation error messages near the input that caused the error.

:Note: You must have a view with the correct JavaScript and `jQuery <https://jquery.com/>`__ 
 script references in place for client side validation. See :doc:`/mvc/models/validation` for more information.
 
The HTML Helper `@Html.ValidationMessageFor <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Rendering/HtmlHelperValidationExtensions/index.html>`__ provides an alternative to this Tag Helper.
  
The `Validation Summary Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/ValidationSummaryTagHelper/index.html>`__  is used to display a summary of validation messages. The `asp-validation-summary`` attribute value can be any of the following:

+-------------------------------+--------------------------------+
|asp-validation-summary         |  Validation messages displayed |  
+===============================+================================+
|ValidationSummary.All          | Property and model level       |
+-------------------------------+--------------------------------+  
|ValidationSummary.ModelOnly    | Model                          |
+-------------------------------+--------------------------------+  
|ValidationSummary.None         | None                           |
+-------------------------------+--------------------------------+  

If the form has validation errors, the browser reports the validation errors and the form data is not posted to the server.

Example
^^^^^^^^^

In the following example, the data model is decorated with ``DataAnnotations`` attributes, which generates validation error messages on the ``<input>`` element.  When a validation error occurs, the Validation Tag Helper displays the error message:

.. literalinclude::  forms/sample/final/ViewModels/RegisterViewModel.cs
  :language: c#

..  literalinclude::  forms/sample/final/Views/Demo/RegisterValidation.cshtml
  :language: HTML
  :emphasize-lines: 4,6,8
  :lines: 1-10

The generated HTML:

.. code-block:: HTML
  :emphasize-lines: 2,3,8,9,12,13
  :linenos:
  
  <form action="/DemoReg/Register" method="post">
    <div class="validation-summary-valid" data-valmsg-summary="true">
    <ul><li style="display:none"></li></ul></div>
    Email:  <input name="Email" id="Email" type="email" value="" 
     data-val-required="The Email field is required." 
     data-val-email="The Email field is not a valid e-mail address." 
     data-val="true"> <br>
    <span class="field-validation-valid" data-valmsg-replace="true" 
     data-valmsg-for="Email"></span><br>
    Password: <input name="Password" id="Password" type="password" 
     data-val-required="The Password field is required." data-val="true"><br>
    <span class="field-validation-valid" data-valmsg-replace="true" 
     data-valmsg-for="Password"></span><br>
    <button type="submit">Register</button>
    <input name="__RequestVerificationToken" type="hidden" value="<removed for brevity>" />
  </form>


The Label Tag Helper
--------------------

The `Label Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/LabelTagHelper/index.html>`__ generates the label caption and ``for=`` attribute on a `<label> <https://www.w3.org/wiki/HTML/Elements/label>`__ element for a property on the model. HTML Helper alternative: ``Html.LabelFor``.

Consider the following ``SimpleViewModel`` and Razor view:

.. literalinclude::  forms/sample/final/ViewModels/SimpleViewModel.cs
  :language: c#

..  literalinclude::  forms/sample/final/Views/Demo/RegisterLabel.cshtml
  :language: HTML
  :emphasize-lines: 4

The following HTML is generated for the ``<label>`` element:

.. code-block:: HTML

 <label for="Email">Email Address</label>  
 
The Label Tag Helper generated the ``for`` attribute value of "Email", which is the name of the property. The caption comes from the ``Display`` attribute. The Label Tag Helper provides the following benefits over a pure HTML label element:

- You automatically get the descriptive label value from the ``Display`` attribute. The intended display name might change over time, and the combination of ``Display`` attribute and Label Tag Helper will apply the ``Display`` everywhere it's used.
- Less markup in source code
- Strong typing with the model property. If the name of the property changes you'll get an error similar to the following:

.. code-block:: HTML

     An error occurred during the compilation of a resource required to process
     this request. Please review the following specific error details and modify
     your source code appropriately.
    
     Type expected
      'SimpleViewModel' does not contain a definition for 'Email' and no
      extension method 'Email' accepting a first argument of type 'SimpleViewModel'
      could be found (are you missing a using directive or an assembly reference?)


The Select Tag Helper
-------------------------

.. question One odd thing I noticed about this is that "asp-for" doesn't require the Model. prefix, but "asp-items" does. If anybody knows why this is, I'd love to hear it.  http://www.exceptionnotfound.net/tag-helpers-in-asp-net-5-an-overview/#selecttaghelper 

The `Select Tag Helper <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/TagHelpers/SelectTagHelper/index.html>`__ generates `select <https://www.w3.org/wiki/HTML/Elements/select>`__ and associated `option <https://www.w3.org/wiki/HTML/Elements/option>`__ elements for properties of your model. ``asp-for`` specifies the model property  name for the `select <https://www.w3.org/wiki/HTML/Elements/select>`__ element  and ``asp-items`` specifies the `option <https://www.w3.org/wiki/HTML/Elements/option>`__ elements.  For example:

.. literalinclude::   forms/sample/final/Views/Home/Index.cshtml
  :language: HTML
  :lines: 4
  :dedent: 3
  
Consider the following view model:

.. literalinclude::  forms/sample/final/ViewModels/CountryViewModel.cs
  :language: c#
  
The ``Index`` method initializes the ``CountryViewModel``, sets the selected country and passes it to the ``Index`` view.

.. literalinclude:: forms/sample/final/Controllers/HomeController.cs
  :language: c#
  :lines: 8-13
  :dedent: 6

The HTTP POST ``Index`` method displays the selection:

.. literalinclude:: forms/sample/final/Controllers/HomeController.cs
  :language: c#
  :lines: 15-27
  :dedent: 6
  
The ``Index`` view:

.. literalinclude::   forms/sample/final/Views/Home/Index.cshtml
  :language: HTML
  :emphasize-lines: 4
  
Which generates the following HTML:

.. code-block:: HTML  
  :linenos:
  :emphasize-lines: 2-6 

  <form method="post" action="/">
    <select id="Country" name="Country">
      <option value="MX">Mexico</option>
      <option selected="selected" value="CA">Canada</option>
      <option value="US">USA</option>
    </select> 
      <br /><button type="submit">Register</button>
    <input name="__RequestVerificationToken" type="hidden" value="<removed for brevity>" />
  </form>

:Note: We do not recommend using ``ViewBag`` or ``ViewData`` with the Select Tag Helper. A view model is more robust at providing MVC metadata and generally less problematic. 

The ``asp-for`` attribute value doesn't require a ``Model`` prefix, while ``asp-items does``:

.. literalinclude::   forms/sample/final/Views/Home/Index.cshtml
  :language: HTML
  :lines: 4
  :dedent: 3

The ``asp-for`` attribute value is the right hand side of a lambda expression. For example, ``asp-for=""Property1"`` becomes ``m => m.Property1`` in the generated code which is why you don't need to prefix with ``Model``.
  
Enum binding
^^^^^^^^^^^^^^

It's often convenient to use an ``enum`` for a list. The ``CountryEnumViewModel`` uses the ``CountryEnum`` to generate a list of countries:

.. literalinclude::  forms/sample/final/ViewModels/CountryEnumViewModel.cs
  :language: c#
  :lines: 3-6
  :dedent: 3
  
.. literalinclude::  forms/sample/final/ViewModels/CountryEnum.cs
  :language: c#
  :lines: 1-4,6,8-

The `GetEnumSelectList <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Rendering/IHtmlHelper/index.html>`__ method generates a `SelectList <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Rendering/SelectList/index.html>`__ object for an enum.

.. literalinclude::   forms/sample/final/Views/Home/IndexEnum.cshtml
  :language: HTML
  :emphasize-lines: 4-6

You can decorate your model with the ``Display`` attribute to get a richer UI:

.. literalinclude::  forms/sample/final/ViewModels/CountryEnum.cs
  :language: c#
  :linenos:
  :emphasize-lines: 5,7

.. image:: forms/_static/enum.png 

Option Group
^^^^^^^^^^^^^^^^^^^^^

The HTML  `<optgroup> <https://www.w3.org/wiki/HTML/Elements/optgroup>`__ element is generated when the view model contains one or more `SelectListGroup  <https://docs.asp.net/projects/api/en/latest/autoapi/Microsoft/AspNet/Mvc/Rendering/SelectListGroup/index.html>`__ objects.

The ``CountryViewModelGroup`` groups the  ``SelectListItem`` elements into the "North America" and "Europe" groups:

.. literalinclude::  forms/sample/final/ViewModels/CountryViewModelGroup.cs
  :language: c#
  :lines: 6-56
  :dedent: 3
  :emphasize-lines: 8-9,17,23,35,41,47,29

The two groups are shown below:

.. image:: forms/_static/grp.png  

Multiple select
^^^^^^^^^^^^^^^^^^^^^

The Select Tag Helper  will automatically generate the `multiple = "multiple" <https://www.w3.org/TR/html-markup/select.html#select.attrs.multiple>`__  attribute if the property specified in the ``asp-for`` attribute is an ``IEnumerable``. For example, given the following model:

.. literalinclude::  forms/sample/final/ViewModels/CountryViewModelIEnumerable.cs
  :language: c#
  :emphasize-lines: 6

With the following view:

.. literalinclude::   forms/sample/final/Views/Home/IndexMultiSelect.cshtml
  :language: HTML
  :emphasize-lines: 4
  
Generates the following HTML:

.. code-block:: HTML  
  :linenos:
  :emphasize-lines: 3

  <form method="post" action="/Home/IndexMultiSelect">
      <select id="CountryCodes" 
      multiple="multiple" 
      name="CountryCodes"><option value="MX">Mexico</option>
  <option value="CA">Canada</option>
  <option value="US">USA</option>
  <option value="FR">France</option>
  <option value="ES">Spain</option>
  <option value="DE">Germany</option>
  </select> 
      <br /><button type="submit">Register</button>
    <input name="__RequestVerificationToken" type="hidden" value="<removed for brevity>" />
  </form>

No selection
---------------------

To allow for no selection, you can add a ``SelectListItem`` to represent no selection. If the property is a `value type <https://msdn.microsoft.com/en-us/library/s1ax56ch.aspx>`__, you'll have to make it `nullable <https://msdn.microsoft.com/en-us/library/2cf62fcy.aspx>`__. The following model includes a null selection:

.. literalinclude::  forms/sample/final/ViewModels/CountryViewModelEmpty.cs
  :language: c#
  :emphasize-lines:  14



Additional Resources
---------------------

- :doc:`Tag Helpers <tag-helpers/intro>`
- `HTML Form element <https://www.w3.org/TR/html401/interact/forms.html>`__
- `Request Verification Token <http://www.asp.net/mvc/overview/security/xsrfcsrf-prevention-in-aspnet-mvc-and-web-pages>`__ 
- :doc:`/mvc/models/model-binding` 
- :doc:`/mvc/models/validation` 
- `data annotations <https://msdn.microsoft.com/en-us/library/system.componentmodel.dataannotations.aspx>`__ 
- `Code snippets for this document <https://github.com/aspnet/Docs/tree/master/aspnet/mvc/views/forms/sample>`_.