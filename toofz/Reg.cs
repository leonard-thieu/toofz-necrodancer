using System;
using Microsoft.Win32;

namespace toofz
{
    /// <summary>
    /// Contains helper and extension methods for working with the registry.
    /// </summary>
    public static class Reg
    {
        /// <summary>
        /// Retrieves an integer value and converts it to a boolean.
        /// </summary>
        /// <param name="key">The registry key to retrieve the value from.</param>
        /// <param name="name">
        /// The name of the value to retrieve. This string is not case-sensitive.
        /// </param>
        /// <param name="defaultValue">The value to return if name does not exist.</param>
        /// <param name="options">
        /// One of the enumeration values that specifies optional processing of the
        /// retrieved value.
        /// </param>
        /// <returns>
        /// If the retrieved value is 1, returns true.
        /// Otherwise, returns false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// The value retrieved is not an integer.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The user does not have the permissions required to read from the registry key.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The Microsoft.Win32.RegistryKey that contains the specified value is closed
        /// (closed keys cannot be accessed).
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The <see cref="RegistryKey" /> that contains the specified value has been marked
        /// for deletion.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options" /> is not a valid <see cref="RegistryValueOptions" />
        /// value; for example, an invalid value is cast to <see cref="RegistryValueOptions" />.
        /// </exception>
        public static bool GetBoolean(this RegistryKey key, string name = null, bool defaultValue = false, RegistryValueOptions options = RegistryValueOptions.None)
        {
            var _defaultValue = defaultValue ? 1 : 0;
            var result = key.GetValue(name, _defaultValue, options);
            if (!(result is int))
                throw new InvalidOperationException("The value retrieved is not an integer.");

            return (int)result == 1;
        }

        /// <summary>
        /// Generic form of RegistryKey.GetValue with all parameters optional. 
        /// If you do not specify a name, the value of (Default) will be retrieved. 
        /// If you do not specify a default value and the specified name is not found, null 
        /// will be returned. 
        /// If you do not specify retrieval options, RegistryValueOptions.None will be used.
        /// </summary>
        /// <typeparam name="T">The return type of the value.</typeparam>
        /// <param name="key">The registry key to retrieve the value from.</param>
        /// <param name="name">
        /// The name of the value to retrieve. This string is not case-sensitive.
        /// </param>
        /// <param name="defaultValue">The value to return if name does not exist.</param>
        /// <param name="options">
        /// One of the enumeration values that specifies optional processing of the 
        /// retrieved value.
        /// </param>
        /// <returns>
        /// NOTE: Returns null if key is null. 
        /// The value associated with name, processed according to the specified options, 
        /// or defaultValue if name is not found.
        /// </returns>
        /// <exception cref="InvalidCastException">
        /// The value could not be cast to T.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The user does not have the permissions required to read from the registry key.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The RegistryKey that contains the specified value is closed (closed keys cannot 
        /// be accessed).
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The RegistryKey that contains the specified value has been marked for deletion.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// options is not a valid RegistryValueOptions value; for example, an invalid 
        /// value is cast to RegistryValueOptions.
        /// </exception>
        public static T GetValue<T>(this RegistryKey key, string name = null, T defaultValue = null, RegistryValueOptions options = RegistryValueOptions.None)
            where T : class
        {
            return (T)key?.GetValue(name, defaultValue, options);
        }

        /// <summary>
        /// Sets the value of the default name/value pair in the registry key, using the
        /// specified registry data type.
        /// </summary>
        /// <param name="key">The registry key to set the default value of.</param>
        /// <param name="value">The data to be stored.</param>
        /// <param name="valueKind">The registry data type to use when storing the data.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key " /> is null.
        /// <paramref name="value" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The type of value did not match the registry data type specified by <paramref name="valueKind" />,
        /// therefore the data could not be converted properly.
        /// </exception>
        /// <exception cref="ObjectDisposedException">
        /// The Microsoft.Win32.RegistryKey that contains the specified value is closed
        /// (closed keys cannot be accessed).
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// The <see cref="RegistryKey" /> is read-only, and cannot be written to; for
        /// example, the key has not been opened with write access.
        /// -or-
        /// The <see cref="RegistryKey" /> object represents a root-level node, and the
        /// operating system is Windows Millennium Edition or Windows 98.
        /// </exception>
        /// <exception cref="System.Security.SecurityException">
        /// The user does not have the permissions required to create or modify registry
        /// keys.
        /// </exception>
        /// <exception cref="System.IO.IOException">
        /// The <see cref="RegistryKey" /> object represents a root-level node, and the
        /// operating system is Windows 2000, Windows XP, or Windows Server 2003.
        /// </exception>
        public static void SetValue(this RegistryKey key, object value, RegistryValueKind valueKind = RegistryValueKind.Unknown)
        {
            key.SetValue("", value, valueKind);
        }

        /// <summary>
        /// Aliases Registry.ClassesRoot.OpenSubKey(string).
        /// Retrieves a subkey as read-only.
        /// </summary>
        /// <param name="name">
        /// The name or path of the subkey to open as read-only.
        /// </param>
        /// <returns>
        /// The subkey requested, or null if the operation failed.
        /// </returns>
        public static RegistryKey HKCR(string name)
        {
            return Registry.ClassesRoot.OpenSubKey(name);
        }

        /// <summary>
        /// Aliases Registry.CurrentUser.OpenSubKey(string).
        /// Retrieves a subkey as read-only.
        /// </summary>
        /// <param name="name">
        /// The name or path of the subkey to open as read-only.
        /// </param>
        /// <returns>
        /// The subkey requested, or null if the operation failed.
        /// </returns>
        public static RegistryKey HKCU(string name)
        {
            return Registry.CurrentUser.OpenSubKey(name);
        }

        /// <summary>
        /// Aliases Registry.LocalMachine.OpenSubKey(string).
        /// Retrieves a subkey as read-only.
        /// </summary>
        /// <param name="name">
        /// The name or path of the subkey to open as read-only.
        /// </param>
        /// <returns>
        /// The subkey requested, or null if the operation failed.
        /// </returns>
        public static RegistryKey HKLM(string name)
        {
            return Registry.LocalMachine.OpenSubKey(name);
        }
    }
}
