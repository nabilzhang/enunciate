package net.sf.enunciate.contract.jaxb.types;

import com.sun.mirror.type.TypeMirror;

/**
 * Type mirror that provides its qname.
 *
 * @author Ryan Heaton
 */
public interface XmlTypeMirror extends TypeMirror {

  /**
   * The (local) name of this xml type.
   *
   * @return The (local) name of this xml type.
   */
  String getName();

  /**
   * The namespace for this xml type.
   *
   * @return The namespace for this xml type.
   */
  String getNamespace();

  /**
   * Whether this type is anonymous.
   *
   * @return Whether this type is anonymous.
   */
  boolean isAnonymous();

}