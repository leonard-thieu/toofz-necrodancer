import 'angular';

declare module 'angular' {
    export namespace ui {
        interface IUrlMatcherFactory {
            /**
             * Sets the default behavior when generating or matching URLs with default parameter values
             *
             * @param value {string} A string that defines the default parameter URL squashing behavior. nosquash: When generating an href with a default parameter value, do not squash the parameter value from the URL slash: When generating an href with a default parameter value, squash (remove) the parameter value, and, if the parameter is surrounded by slashes, squash (remove) one slash from the URL any other string, e.g. "~": When generating an href with a default parameter value, squash (remove) the parameter value from the URL and replace it with this string.
             */
            defaultSquashPolicy(value: string | boolean): void;
        }
    }
}