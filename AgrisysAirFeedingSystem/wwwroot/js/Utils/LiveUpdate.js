/**
 * A number, or a string containing a number.
 * @typedef {{key: string, timeStamp: string, value: int}} valueUpdate
 * @typedef {{key: string, handler:dataHandler}} keyHandler
 */

class Connection {
    /** @type {Map<String, dataHandler>} */
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
        this.targets.set(target.key, target.handler);
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
            let target = this.targets.get(value.key);

            if (target !== undefined) {
                target.handleUpdate(value);
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
        let target = this.targets.get(value.key);
        
        if (target !== undefined) {
            target.handleUpdate(value);
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



class LiveUpdateDataExtractor extends DataExtractor {
    /**
     *
     */
    constructor() {
        super();
        
        //TODO: make this dynamic
        this.keys = ["sensorKey","sensorHub","sensorDataType","sensorInsertMethod","sensorDataScaleFactor","sensorOptions"]
    }

    extract(node) {
        /** @type {Map<string, string | undefined>} */
        let map = new Map();
        for (const key of this.keys) {
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

class InsertContentHandler extends dataHandler {
    /**
     * provides a way to insert data into the content of an element
     * @param {HTMLElement} target
     * @param {dataFormatter} formatter
     */
    constructor(target, formatter) {
        super(formatter);
        this.target = target;
    }

    handleUpdate(value) {
        this.target.innerText = this.formatHelper(value);
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

class NumberFormatter extends dataFormatter {
    /**
     * @param {Map<string,string>} options
     */
    constructor(options) {
        super();
        this.scaleFactor = options.get("sensorDataScaleFactor");
    }
    /**
     *
     * @returns {string}
     */
    format(data) {
        if (this.scaleFactor !== undefined) {
            data = (data.value / this.scaleFactor);
        }
        
        return data.value.toString();
    }
}

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
        return this.Options[data.value] ?? "undefined";
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
     * @param {Map<HTMLElement, doubleLinkedTreeNode>} nodes
     */
    traverse(startNode,nodes,) {
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
    let dataType = options.get("sensorDataType");
    let dataInsertMethod = options.get("sensorInsertMethod") ?? "content";

    /** @type {dataFormatter | undefined} */
    let formatter;

    switch (dataType) {
        case "number":
            formatter = new NumberFormatter(options);
            break;
        case "enum":
            formatter = new EnumFormatter(options);
            break;
    }

    /** @type {dataHandler | undefined} */
    let handler;

    switch (dataInsertMethod) {
        case "content":
            handler = new InsertContentHandler(target,formatter);
            break
    }
    
    return handler;
}


/** @type {[Connection]} */
let connections = [];

function startConnection(){
    let traverser = new domUpTraverser(new RootTraversalLimiter(), new LiveUpdateDataExtractor());
    let dataFiller = new nodeDataFaller();
    /** @type {[HTMLElement]} */
    let targets =  Array.from(document.querySelectorAll("[data-sensor-key]"));
    
    let result = traverser.traverse(targets, 
        (node) => targets.includes(node));

    dataFiller.traverse(result.nodes.get(document.querySelectorAll("body")[0]),result.nodes);

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