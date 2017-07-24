/// <reference types="jquery" />

interface JQueryStatic {
    typeahead: Bootstrap3Typeahead.TypeaheadStatic<any>;
}

interface JQuery {
    /**
     * Initializes an input with a typeahead.
     */
    typeahead(): JQuery;
    /**
     * Destroys previously initialized typeaheads. This entails reverting DOM modifications and removing event handlers
     */
    typeahead(action: 'destroy'): JQuery;
    /**
     * To get the currently active item, you will get a String or a JSON object depending on how you initialized typeahead.
     * Works only for the first match.
     */
    typeahead<T>(action: 'getActive'): T;
    typeahead<T>(options: Bootstrap3Typeahead.Options<T>): JQuery;
}

declare namespace Bootstrap3Typeahead {
    interface TypeaheadStatic<T> {
        defaults: Options<T>;
        Constructor: Typeahead<T>;
        noConflict(): this;
    }

    class Typeahead<T> {
        constructor(element: string | JQuery, options: Bootstrap3Typeahead.Options<T>);

        $element: JQuery;
        options: Bootstrap3Typeahead.Options<T>;
        autoSelect: boolean;
        source: T[];
        $menu: JQuery;
        $appendTo: JQuery | null;
        fitToElement: boolean;
        shown: boolean;
        showHintOnFocus: boolean | 'all';
        afterSelect: (item: T) => void;
        addItem: boolean;
        value: any;

        select(): this;

        updater(item: T): T;

        setSource(source: T[]): void;

        show(): this;

        hide(): this;

        lookup(query: string): this | void;

        process(items: T[]): this;

        // Default implementation returns number
        matcher(item: T): number;

        sorter(items: T[]): T[];

        highlighter(text: string, item: T): string;

        render(items: T[]): this;

        displayText(item: T): string;

        next(event: any): void;

        prev(event: any): void;

        listen(): void;

        destroy(): void;

        eventSupported(eventName: string): boolean;

        move(e: JQueryEventObject): void;

        keydown(e: JQueryEventObject): void;

        keypress(e: JQueryEventObject): void;

        input(e: JQueryEventObject): void;

        keyup(e: JQueryEventObject): void;

        focus(e: JQueryEventObject): void;

        blur(e: JQueryEventObject): void;

        click(e: JQueryEventObject): void;

        mouseenter(e: JQueryEventObject): void;

        mouseleave(e: JQueryEventObject): void;

        mousedown(e: JQueryEventObject): void;
    }

    interface Options<T> {
        /**
         * The data source to query against.
         * May be an array of strings, an array of JSON object with a name property or a function.
         * The function accepts two arguments, the query value in the input field and the process callback.
         * The function may be used synchronously by returning the data source directly or asynchronously via the process callback's single argument.
         */
        source?: ((query: string, process: (data: T[]) => void) => void | T[]) | T[];
        /**
         * The max number of items to display in the dropdown. Can also be set to 'all'
         *
         * @default 8
         */
        items?: number | 'all';
        /**
         * The minimum character length needed before triggering autocomplete suggestions.
         * You can set it to 0 so suggestion are shown even when there is no text when lookup function is called.
         *
         * @default 1
         */
        minLength?: number;
        /**
         * If hints should be shown as soon as the input gets focus.
         * If set to true, all match will be shown.
         * If set to "all", it will display all hints, not filtering them by the current text.
         * This can be used when you want an input that behaves a bit like a combo box plus auto completion as you type to filter the choices.
         *
         * @default false
         */
        showHintOnFocus?: boolean | 'all';
        /**
         * Number of pixels the scrollable parent container scrolled down (scrolled out the viewport).
         *
         * @default 0
         */
        scrollHeight?: number | (() => number);
        /**
         * The method used to determine if a query matches an item.
         * Accepts a single argument, the item against which to test the query.
         * Access the current query with this.query.
         * Return a boolean true if query is a match.
         */
        matcher?: (this: Typeahead<T>, item: T) => boolean;
        /**
         * The method used to return selected item.
         * Accepts a single argument, the item and has the scope of the typeahead instance.
         */
        updater?: (this: Typeahead<T>, item: T) => T;
        /**
         * Method used to highlight autocomplete results.
         * Accepts a single argument item and has the scope of the typeahead instance.
         * Should return html.
         */
        highlighter?: (this: Typeahead<T>, text: string, item: T) => string | JQuery;
        /**
         * Method used to get textual representation of an item of the sources.
         * Accepts a single argument item and has the scope of the typeahead instance.
         * Should return a String.
         */
        displayText?: (this: Typeahead<T>, item: T) => string;
        /**
         * Allows you to dictate whether or not the first suggestion is selected automatically.
         * Turning autoselect off also means that the input won't clear if nothing is selected and enter or tab is hit.
         *
         * @default true
         */
        autoSelect?: boolean;
        /**
         * Call back function to execute after selected an item. It gets the current active item in parameter if any.
         */
        afterSelect?: (item: T | '') => void;
        /**
         * Adds a delay between lookups.
         *
         * @default 0
         */
        delay?: number;
        /**
         * By default, the menu is added right after the input element.
         * Use this option to add the menu to another div.
         * It should not be used if you want to use bootstrap dropup or dropdown-menu-right classes.
         *
         * @default null
         */
        appendTo?: JQuery | null;
        /**
         * Set to true if you want the menu to be the same size than the input it is attached to.
         *
         * @default false
         */
        fitToElement?: boolean;
        /**
         * Adds an item to the end of the list, for example "New Entry".
         * This could be used, for example, to pop a dialog when an item is not found in the list of data.
         *
         * @default false
         * @see {@link http://cl.ly/image/2u170I1q1G3A/addItem.png}
         */
        addItem?: false | any;
    }
}