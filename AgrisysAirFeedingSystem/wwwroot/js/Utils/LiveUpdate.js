/**
 * A number, or a string containing a number.
 * @typedef {{key: string, timeStamp: string, value: int}} valueUpdate
 * @typedef {{key: string, handler:dataHandler}} keyHandler
 */

class Connection {
    /** @type {Map<String, [dataHandler]>} */
    targets = new Map();
    /**
     * Represents a book.
     * @param {string} url - The author of the book.
     * @param {Array.<keyHandler>} targets - The author of the book.
     */
    constructor(url, targets = []) {
        this.url = url;
        
        for (const target of targets) {
            this.addTarget(target);
        }
    }

    /**
     * Add a target to the connection
     * @param {keyHandler} target - The author of the book.
     */
    addTarget(target) {
        /** @type {[dataHandler]|undefined} */
        let handlerCollection = this.targets.get(target.key);
        if (handlerCollection === undefined) {
            handlerCollection = [];
            this.targets.set(target.key, handlerCollection);
        }
        handlerCollection.push(target.handler);
        
        //this.targets.set(target.key, target.handler);
    }

    /**
     * Remove a target to the connection
     * @param {liveUpdateData} target - The author of the book.
     */
    removeTarget(target) {
        this.targets.delete(target.key);
    }
    
    connect() {        
        let keysIter = this.targets.keys();
        /** @type {string} */
        let keys = keysIter.next()?.value;
        
        for (const key of keysIter) {
            keys += ","+key;
        }
        
        console.log(keys);
        
        let builder = new signalR.HubConnectionBuilder()
            .withUrl(this.url+"?"+new URLSearchParams({keys}));
        
        this.connection = builder.build();
        this.connection.on("valueUpdate", (value)=>{
            console.log(value)
            let targets = this.targets.get(value.key);

            if (targets !== undefined) {
                for (const target of targets) {
                    target.handleUpdate(value);
                }
            }else{
                console.log("target not found", value.key);
            }
        });
        this.connection.start().catch(function (err) {
            return console.error(err.toString());
        });
    }

    /**
     * 
     * @param {valueUpdate} value
     */
    handleUpdate(value) {
        console.log(value)
        /** @type {[dataHandler]|undefined} */
        let targets = this.targets.get(value.key);
        
        if (targets !== undefined) {
            for (const target of targets){
                target.handleUpdate(value);
            }
        }else{
            console.log("target not found", value.key);
        }
    }
}

class liveUpdateData {
    /**
     * Represents a book.
     * @param {string} key - The author of the book.
     * @param {string} hub - The author of the book.
     * @param {HTMLElement} target - The author of the book.
     */
    constructor(key,hub,target) {
        this.key = key;
        this.hub = hub;
        this.target = target;
    }
}

class doubleLinkedTreeNode {
    constructor(data) {
        this.data = data;
        /** @type {doubleLinkedTreeNode} */
        this.parent = null;
        this.children = [];
    }

    addChild(child) {
        this.children.push(child);
        child.parent = this;
    }

    removeChild(child) {
        let index = this.children.indexOf(child);
        this.children.splice(index, 1);
    }
}

let counter = 0;


class TraversalLimiter {    
    /**
     * 
     * @param {doubleLinkedTreeNode} node 
     */
    canTraverse(node) {
        return true;
    }
}

class RootTraversalLimiter extends TraversalLimiter {
    
    /**
     *
     */
    constructor() {
        super();
        this.root = document.querySelectorAll("body")[0];
    }

    canTraverse(node) {
        return node.data.target !== this.root;
    }
}

class DataExtractor {
    /**
     *
     * @param {HTMLElement} node
     * @returns {Object}
     */
    extract(node) {
        return {};
    }
}

/** @type {[string]} */
let possibleKeys = [];
class LiveUpdateDataExtractor extends DataExtractor {
    extract(node) {
        /** @type {Map<string, string | undefined>} */
        let map = new Map();
        for (const key of possibleKeys) {
            let value = node.dataset[key];
            if (value !== undefined) {
                map.set(key, value);
            }
        }
        
        return {target: node, options:map};
    }
}


class dataHandler {
    /**
     * Provides a way to handle incoming data from the server
     * @param {dataFormatter} formatter
     */
    constructor(formatter) {
        this.formatter = formatter;
    }

    /**
     * Handle the incoming data
     * @param {valueUpdate} value
     */
    handleUpdate(value) {
        console.log(value);
    }

    /**
     * @param {valueUpdate} update
     * @returns {string}
     */
    formatHelper (update) {
        if (this.formatter !== undefined) {
            return this.formatter.format(update);
        }
        return update.value.toString();
    }
}

possibleKeys.push("contentFormat","contentPrefix","contentSuffix");
class InsertContentHandler extends dataHandler {
    /**
     * provides a way to insert data into the content of an element
     * @param {HTMLElement} target
     * @param {Map<string,string>} Options
     * @param {dataFormatter} formatter
     */
    constructor(target, formatter,Options) {
        super(formatter);
        this.target = target;

        this.format = Options.get("contentFormat");
        this.prefix = Options.get("contentPrefix");
        this.suffix = Options.get("contentSuffix");
    }

    handleUpdate(update) {
        let output = "";
        
        let value = this.formatHelper(update);
        
        
        if (this.format !== undefined) 
        {
            output += this.format.replace("{value}",value.toString());
        }
        else if (this.prefix == null || this.suffix == null)
        {

            if (this.prefix != null) output += this.prefix;
            output += value;
            if (this.suffix != null) output += this.suffix;
        }
        else
        {
            output += value;
        }
        
        this.target.innerText = output;
    }
}

possibleKeys.push("styleHandlerProperty","styleHandlerUnit");
class cssStyleHandler extends dataHandler {
    /**
     * provides a way to insert data into the content of an element
     * @param {HTMLElement} target
     * @param {dataFormatter} formatter
     * @param {Map<string,string>} Options
     */
    constructor(target, formatter,Options) {
        super(formatter);
        this.target = target;

        this.property = Options.get("styleHandlerProperty");
        this.unit = Options.get("styleHandlerUnit");
    }

    handleUpdate(value) {
        this.target.style[this.property] = this.formatHelper(value) + this.unit;
    }
}

possibleKeys.push("cssClass","oldCssClass");
class cssClassHandler extends dataHandler {
    /**
     * provides a way to insert data into the content of an element
     * @param {HTMLElement} target
     * @param {dataFormatter} formatter
     * @param {Map<string,string>} Options
     */
    constructor(target, formatter,Options) {
        super(formatter);
        this.target = target;

        this.class = Options.get("cssClass");
        this.oldClass = Options.get("oldCssClass") ?? "";
    }

    handleUpdate(update) {
        let formattedValue = this.formatHelper(update)
        
        if (this.class !== undefined) {
            formattedValue = this.class.slice().replace("{value}", formattedValue);
        }
        
        this.target.classList.add(formattedValue);
        if (this.oldClass !== "") this.target.classList.remove(this.oldClass);
        
        //TODO: doesn't remove class set by aspnet
        this.oldClass = formattedValue;
    }
}
class CustomHandler extends dataHandler {
    /**
     * provides a way to insert data into the content of an element
     * @param {HTMLElement} target
     * @param {dataFormatter} formatter
     * @param {Map<string,string>} Options
     */
    constructor(target, formatter,Options) {
        super(formatter);
        this.target = target;
        
        let initial = this.target.dataset["customInitial"];

        this.target.dataset["customInitial"] = undefined;

        this.dispatch({target,initial}, "customHandleInit");
    }

    handleUpdate(update) {
        // Dispatch the event.
        this.dispatch(
            {
                formatted:this.formatHelper(update), 
                target: this.target
                ,...update
            }
        )
    }
    
    dispatch(event,name = "valueUpdate"){
        // Dispatch the event.
        this.target.dispatchEvent(new CustomEvent(name,
            {
                bubbles: true,
                ...event
            }
        ));
    }
}



class dataFormatter {
    /**
     *
     * @param {valueUpdate} update
     * @returns {string}
     */
    format(update) {
        return "sample data";
    }
}

possibleKeys.push("sensorScaleFactor","sensorScaleDigit");
class NumberFormatter extends dataFormatter {
    /**
     * @param {Map<string,string>} options
     */
    constructor(options) {
        super();
        this.scaleFactor = parseInt(options.get("sensorScaleFactor"));        
        this.digit = parseInt(options.get("sensorScaleDigit"));

        if (this.scaleFactor === undefined) {
            throw new Error("no scale factor found");
        }
    }
    /**
     *
     * @returns {string}
     */
    format(update) {
        let result = (update.value / this.scaleFactor);

        if (this.digit !== undefined) {
            let digitMultiplier = Math.pow(10, this.digit);
            result = Math.floor((result + Number.EPSILON) * digitMultiplier) / digitMultiplier;
        }
        
        return result.toString();
    }
}



possibleKeys.push("sensorMapInMin","sensorMapInMax","sensorMapOutMin","sensorMapOutMax");
class NumberMapFormatter extends dataFormatter {
    /**
     * @param {Map<string,string>} options
     */
    constructor(options) {
        super();
        this.inputMin = parseInt(options.get("sensorMapInMin")??0);
        this.inputMax = parseInt(options.get("sensorMapInMax"));
        this.outputMin = parseInt(options.get("sensorMapOutMin")??0);
        this.outputMax = parseInt(options.get("sensorMapOutMax"));
    }
    /**
     *
     * @returns {string}
     */
    format(data) {
        let value = data.value;
        
        //map value
        value = (value - this.inputMin) * (this.outputMax - this.outputMin) / (this.inputMax - this.inputMin) + this.outputMin;

        return value.toString();
    }
}

possibleKeys.push("sensorLevels");
class LevelFormatter extends dataFormatter {
    /**
     * @param {Map<string,string>} options
     */
    constructor(options) {
        super();
        
        let levelsStr = options.get("sensorLevels");
        
        if (levelsStr === undefined) throw new Error("no levels found");
        
        let levels = levelsStr.split(",");
        /** @type {Map<number,string>} */
        let map = new Map();
        for (let i = 0; i < levels.length; i++) {
            let [key, value] = levels[i].split(":");
            map.set(parseInt(key),value);
        }

        
        this.levels = map;
    }
    /**
     *
     * @returns {string}
     */
    format(data) {
        let value = "Unknown";

        for (const entry of this.levels.entries()) {
            if (entry[0] > data.value) {
                break;
            }

            value = entry[1];
        }

        return value;
    }
}

possibleKeys.push("sensorOptions");
class EnumFormatter extends dataFormatter {
    /**
     *
     * @param {Map<string,string>} settings
     */
    constructor(settings) {
        super();
        
        let options = settings.get("sensorOptions");
        
        if (options !== undefined) {
            this.Options = options.split(",");
        }else{
            throw new Error("no options found");
        }
    }
    /**
     *
     * @returns {string}
     */
    format(data) {
        //TODO: discuss if the default value should be the empty or undefined
        return this.Options[data.value] ?? throw new Error("value out of range");
    }
}


/**
 *
 * @param {string} error
 * @returns {void}
 */
function handleFatalError(error) {
    throw new Error(error);
}

class domUpTraverser {
    /** @type {TraversalLimiter} */
    traversalLimiter;
    /**
     * 
     * @param {TraversalLimiter} traversalLimiter 
     * @param {DataExtractor} dataExtractor
     */
    constructor(traversalLimiter, dataExtractor) {
        this.traversalLimiter = traversalLimiter;
        this.dataExtractor = dataExtractor;
    }
    
    /**
     * @param {function(HTMLElement):boolean} contextLimiter 
     * @param {[HTMLElement]} startElements
     * @returns {{nodes:Map<HTMLElement,doubleLinkedTreeNode>,inputs:[doubleLinkedTreeNode]}}
     */
    traverse(startElements,contextLimiter) {
        /** @type {Map<HTMLElement, doubleLinkedTreeNode>} */
        let nodes = new Map()
        let inputs = this.getStartStartNodes(startElements);
        
        let output = {inputs:[...inputs]};
        
        while (inputs.length > 0) {
            let tmp = [];
            for (let i = 0; i < inputs.length; i++) {
                /** @type {doubleLinkedTreeNode} */
                let input = inputs[i];
                /** @type {HTMLElement} */
                let targetElement = input.data.target
                let parentElement = targetElement.parentElement;

                // Prevent nesting by checking if the parent is a start element
                if (contextLimiter(parentElement)) {
                    handleFatalError("nesting is not supported");
                }

                /** @type {doubleLinkedTreeNode | undefined} */
                let parent = nodes.get(parentElement);

                // create parent if it does not exist
                if (parent === undefined) {
                    parent = new doubleLinkedTreeNode(this.dataExtractor.extract(parentElement));

                    console.log("parent", parentElement);
                    // Save the node
                    nodes.set(parentElement, parent)
                    
                    // add the parent to the targets if it traversable
                    if (this.traversalLimiter.canTraverse(parent)) {
                        tmp.push(parent);
                    }
                }

                parent.addChild(input);
            }

            inputs = tmp;
        }
        
        return Object.assign(output,{nodes});
    }

    /**
     * @param {[HTMLElement]} startElements
     * @returns {[doubleLinkedTreeNode]}
     */
    getStartStartNodes(startElements) {
        let nodes = [];
        for (let i = 0; i < startElements.length; i++) {
            nodes.push(new doubleLinkedTreeNode(this.dataExtractor.extract(startElements[i])));
        }

        return nodes;
    }
}


class nodeDataFaller {
    /**
     * 
     */
    constructor() {

    }

    /**
     * @param {doubleLinkedTreeNode} startNode
     */
    traverse(startNode) {
        /** @type {[doubleLinkedTreeNode]} */
        let inputs =  [startNode]        
        
        while(inputs.length > 0) {
            /** @type {doubleLinkedTreeNode[]} */
            let tmp = [];

            for (let i = 0; i < inputs.length; i++) {
                let node = inputs[i];
                let children = node.children;
                for (let j = 0; j < children.length; j++) {
                    let child = children[j];
                    
                    child.data.options= mapCombiner(child.data.options,node.data.options);
                    
                    console.log(child.data);
                }

                tmp = tmp.concat(children);
            }

            inputs = tmp;
        }
    }
}

/**
 * Combine two maps, the first map have priority over the second
 * @param {Map<string,string>} map1
 * @param {Map<string,string>} map2
 * @returns {Map<string,string>}
 */
function mapCombiner(map1, map2) {
    let result = new Map(map1);
    for (const [key, value] of map2) {
        if (result.get(key) === undefined) {
            result.set(key, value);
        }
    }
    return result;
}

possibleKeys.push("sensorFormat","sensorHandler");
/**
 * Constructs a handler from the options
 * @param {HTMLElement} target
 * @param {Map<string,string>} options
 * @returns {dataHandler|undefined}
 * @constructor
 */
function BuildHandler(target,options) {
    //if no datatype is set, the handler will just insert the raw data
    /** @type {string | undefined} */
    let format = options.get("sensorFormat");
    let Handler = options.get("sensorHandler") ?? "content";

    /** @type {dataFormatter | undefined} */
    let formatter;

    switch (format) {
        case "number":
            formatter = new NumberFormatter(options);
            break;
        case "numberMap":
            formatter = new NumberMapFormatter(options);
            break;
        case "enum":
            formatter = new EnumFormatter(options);
            break;
        case "level":
            formatter = new LevelFormatter(options);
            break;
    }

    /** @type {dataHandler | undefined} */
    let handler;

    switch (Handler) {
        case "content":
            handler = new InsertContentHandler(target,formatter,options);
            break
        case "cssClass":
            handler = new cssClassHandler(target,formatter,options);
            break
        case "cssStyle":
            handler = new cssStyleHandler(target,formatter,options);
            break
        case "custom":
            handler = new CustomHandler(target,formatter,options);
            break
    }
    
    return handler;
}


/** @type {[Connection]} */
let connections = [];

possibleKeys.push("sensorHub","sensorKey");
function startConnection(){
    let traverser = new domUpTraverser(new RootTraversalLimiter(), new LiveUpdateDataExtractor());
    let dataFiller = new nodeDataFaller();
    /** @type {[HTMLElement]} */
    let targets =  Array.from(document.querySelectorAll("[data-sensor-key]"));
    
    let result = traverser.traverse(targets, 
        (node) => targets.includes(node));

    dataFiller.traverse(result.nodes.get(document.querySelectorAll("body")[0]));

     for (const targetsKey of result.inputs) {
         let hub = targetsKey.data.options.get("sensorHub");
         if (hub === undefined) {
             throw new Error("no hub found");
         }         
         
        /** @type {Connection|undefined} */
        let connection = connections.find(x => x.url === hub);

        if (connection === undefined) {
            connection = new Connection(hub);
            connections.push(connection);
        }

        //find the key
        let Options = targetsKey.data.options;
        let key = Options.get("sensorKey");
        
        if (key === undefined) {
            throw new Error("no key found");
        }
        
        let targetData = {key, handler:BuildHandler(targetsKey.data.target,Options)};
        
        connection.addTarget(targetData);
    }

    for (const connection of connections) {
        connection.connect();
    }
}
startConnection();