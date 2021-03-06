/**
 * Copyright © 2006-2016 Web Cohesion (info@webcohesion.com)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
package com.webcohesion.enunciate.modules.spring_web.model;


import com.webcohesion.enunciate.modules.spring_web.EnunciateSpringWebContext;

import javax.lang.model.element.ExecutableElement;

/**
 * A resource parameter with explicit values.
 * 
 * @author Ryan Heaton
 */
public class ExplicitRequestParameter extends RequestParameter {

  private final String docValue;
  private final String paramName;
  private final ResourceParameterType type;
  private final boolean multivalued;
  private final ResourceParameterConstraints constraints;

  public ExplicitRequestParameter(ExecutableElement method, String docValue, String paramName, ResourceParameterType type, EnunciateSpringWebContext context) {
    this(method, docValue, paramName, type, false, new ResourceParameterConstraints.UnboundString(), context);
  }

  public ExplicitRequestParameter(ExecutableElement method, String docValue, String paramName, ResourceParameterType type, boolean multivalued, ResourceParameterConstraints constraints, EnunciateSpringWebContext context) {
    super(method, context.getContext().getProcessingEnvironment());
    this.docValue = docValue;
    this.paramName = paramName;
    this.type = type;
    this.multivalued = multivalued;
    this.constraints = constraints;
  }

  public String getDocValue() {
    return docValue;
  }

  @Override
  public String getParameterName() {
    return paramName;
  }

  @Override
  public String getDefaultValue() {
    return null;
  }

  @Override
  public String getTypeName() {
    return this.type.toString().toLowerCase();
  }

  @Override
  public boolean isMultivalued() {
    return this.multivalued;
  }

  @Override
  protected ResourceParameterConstraints loadConstraints() {
    return this.constraints;
  }

  @Override
  protected ResourceParameterDataType loadDataType() {
    return ResourceParameterDataType.STRING;
  }
}
