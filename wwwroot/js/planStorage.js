// wwwroot/js/planStorage.js

window.PlanStorage = {
    // Get all completed videos
    getCompletedVideos: function () {
        let data = localStorage.getItem("completedVideos");
        return data ? JSON.parse(data) : {};
    },

    // Mark a video as completed
    markVideoCompleted: function (planId, videoId) {
        let completed = this.getCompletedVideos();
        if (!completed[planId]) completed[planId] = [];
        if (!completed[planId].includes(videoId)) completed[planId].push(videoId);
        localStorage.setItem("completedVideos", JSON.stringify(completed));
    },

    // Check if a video is completed
    isVideoCompleted: function (planId, videoId) {
        let completed = this.getCompletedVideos();
        return completed[planId] ? completed[planId].includes(videoId) : false;
    },

    // Get progress for a plan
    getPlanProgress: function (planId, totalLessons) {
        let completed = this.getCompletedVideos();
        let count = completed[planId] ? completed[planId].length : 0;
        return { completed: count, total: totalLessons };
    },

    // Reset completed videos for a plan
    resetPlan: function (planId) {
        let completed = this.getCompletedVideos();
        if (completed[planId]) {
            delete completed[planId];
            localStorage.setItem("completedVideos", JSON.stringify(completed));
        }
    }
};
