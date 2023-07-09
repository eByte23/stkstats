

let realsft = {

    scroll: () => {

        window.scrollTo(0, document.body.scrollHeight);
    },
    scrollElement: (elementSelector) => {
        let element = document.querySelector(elementSelector);
        element.scrollTop = element.scrollHeight;
    },
    exists: (elementSelector, useXpath) => {
        let element =document.querySelector(elementSelector);
        
        return (element != null || element != undefined) && element.length != 0;
    },
    remove: (elementSelector)=>{
        document.querySelector(elementSelector).remove();
    }

};

window.__realsft = realsft;