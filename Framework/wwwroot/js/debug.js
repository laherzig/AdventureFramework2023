window.debug = {
    objRef: null,

    slides: {},

    init: function (objRef) {
        this.objRef = objRef;
    },
    action: function (action) {
        this.objRef.invokeMethodAsync("Action", action);
    },
    actions: function (actions) {
        this.objRef.invokeMethodAsync("Actions", actions);
    },
    // does not seem to work properly
    update: function () {
        this.objRef.invokeMethodAsync("Update");
    },

    // shorthands for some actions

    // cast all params to string, just in case
    route: function (id) {
        this.action(["Route", String(id)]);
    },
    setGameState: function (id, val) {
        this.action(["SetGameState", String(id), String(val)]);
    },

    addItem: function (id) {
        this.action(["AddItem", String(id)]);
    },

    removeItem: function (id) {
        this.action(["RemoveItem", String(id)]);
    },

    playSound: function (id) {
        this.action(["PlaySound", String(id)]);
    },

    playMusic: function (id) {
        this.action(["PlayMusic", String(id)]);
    },

    stopMusic: function (id) {
        this.action(["StopMusic", String(id)]);
    },

    getSlideId: function () {
        this.objRef
            .invokeMethodAsync("GetCurrentSlide")
            .then((id) => console.log(id));
    },
    getGameState: function (id) {
        this.objRef
            .invokeMethodAsync("GetGameState", String(id))
            .then((state) => console.log(state));
    },
    getSlideContent: function () {
        this.objRef
            .invokeMethodAsync("GetCurrentSlideContent")
            .then((content) => console.log(JSON.stringify(content, 0, 2)));
    },
    getButtons: function () {
        this.objRef
            .invokeMethodAsync("GetCurrentSlideButtons")
            .then((buttons) => console.log(JSON.stringify(buttons, 0, 2)));
    },
    reRoute: function () {
        this.objRef.invokeMethodAsync("ReRoute");
    },
    items: function () {
        this.objRef
            .invokeMethodAsync("GetItems")
            .then((items) => console.log(items));
    },
    redbull: function (amount = 0) {
        this.objRef
            .invokeMethodAsync("RedBull", amount)
            .then((res) => console.log(res));
    },
    playVideo: function (points, path) {
        this.action(["PlayVideo", points, path]);
    },
    getSaveString: function () {
        this.objRef
            .invokeMethodAsync("GetSaveString")
            .then((saveString) => console.log(saveString));
    },
    loadSaveString: function (saveString) {
        this.objRef.invokeMethodAsync("LoadSaveString", saveString);
    },
};
